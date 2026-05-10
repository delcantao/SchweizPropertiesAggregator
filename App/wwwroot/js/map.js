// O MapLibre estará disponível globalmente como 'maplibregl'



const map = new maplibregl.Map({
  container: "map",

  style: "https://tiles.openfreemap.org/styles/liberty",

  center: [8.2275, 46.8182],
  zoom: 8,
});

let highlightedCardId = null;

const highlightedCardStyle = {
  outline: "3px solid #38bdf8",
  outlineOffset: "2px",
};

function clearHighlightedCard() {
  if (!highlightedCardId) {
    return;
  }

  const previous = document.querySelector(
    `[data-property-id="${highlightedCardId}"]`,
  );

  if (previous) {
    previous.style.outline = "";
    previous.style.outlineOffset = "";
  }

  highlightedCardId = null;
}

function highlightCard(propertyId) {
  if (!propertyId) {
    return;
  }

  if (highlightedCardId === propertyId) {
    return;
  }

  clearHighlightedCard();

  const card = document.querySelector(`[data-property-id="${propertyId}"]`);
  if (!card) {
    return;
  }

  card.style.outline = highlightedCardStyle.outline;
  card.style.outlineOffset = highlightedCardStyle.outlineOffset;
  highlightedCardId = propertyId;
}

map.on("error", (e) => {
  console.log("error debug", e);
});

map.on("moveend", async () => {
  refresh();
});

function refresh() {
  const bounds = map.getBounds();

  const west = bounds.getWest();
  const south = bounds.getSouth();
  const east = bounds.getEast();
  const north = bounds.getNorth();

  refreshCards(west, south, east, north);
  refreshMap(west, south, east, north);
}
map.on("load", async () => {
  const response = await fetch("/api/properties/map");

  const geojson = await response.json();

  map.addSource("properties", {
    type: "geojson",
    data: geojson,

    cluster: true,
    clusterMaxZoom: 14,
    clusterRadius: 50,
  });

  addPropertyLayers();
  // refresh();
});

function refreshCards(west, south, east, north) {
  htmx.ajax(
    "GET",
    `/properties/cards?west=${west}&south=${south}&east=${east}&north=${north}`,
    {
      target: "#cards",
      swap: "innerHTML",
    },
  );
}

async function refreshMap(west, south, east, north) {
  const response = await fetch(
    `/api/properties/map?west=${west}&south=${south}&east=${east}&north=${north}`,
  );

  const geojson = await response.json();
  const source = map.getSource("properties");

  if (!source) {
    console.error("Source not found", source);
    return;
  }
  source.setData(geojson);
}

function addPropertyLayers() {
  map.addLayer({
    id: "clusters",

    type: "circle",

    source: "properties",

    filter: ["has", "point_count"],

    paint: {
      "circle-stroke-width": 2,
      "circle-stroke-color": "#fff",
      // 'circle-color': '#ff0000',
      "circle-radius": 22,
      "circle-color": [
        "step",
        ["get", "point_count"],
        "#51bbd6",
        10,
        "#f1f075",
        50,
        "#f28cb1",
      ],
    },
  });

  map.addLayer({
    id: "cluster-count",

    type: "symbol",

    source: "properties",

    filter: ["has", "point_count"],

    layout: {
      "text-field": ["get", "point_count_abbreviated"],
      "text-size": 12,
    },
  });

  map.addLayer({
    id: "unclustered-point",

    type: "circle",

    source: "properties",

    filter: ["!", ["has", "point_count"]],

    paint: {
      "circle-color": "#11b4da",
      "circle-radius": 10,
      "circle-stroke-width": 2,
      "circle-stroke-color": "#fff",
    },
  });

  map.on("click", "unclustered-point", (e) => {
    const feature = e.features[0];

    const coordinates = feature.geometry.coordinates;

    const properties = feature.properties;
    const images = JSON.parse(feature.properties.image); 
    const popupHtml = `
        <div class="popup" hover-id="${feature.properties.id}">

            <img
                src="${images[0]}"
                style="width:100%; border-radius:12px;" />

            <h3>${properties.title}</h3>

            <p>
                CHF ${Number(properties.price).toLocaleString()}
            </p>

        </div>
    `;

    new maplibregl.Popup().setLngLat(coordinates).setHTML(popupHtml).addTo(map);
  });

  map.on("click", "clusters", (e) => {
    const currentZoom = map.getZoom();
    const coordinates = e.features[0].geometry.coordinates;
    map.easeTo({ center: coordinates, zoom: currentZoom + 2 });
  });

  map.on("mouseenter", "unclustered-point", () => {
    map.getCanvas().style.cursor = "pointer";
  });

  map.on("mousemove", "unclustered-point", (e) => {
    const feature = e.features?.[0];
    highlightCard(feature?.properties?.id?.toString());
  });

  map.on("mouseleave", "unclustered-point", () => {
    map.getCanvas().style.cursor = "";
    clearHighlightedCard();
  });
}

 