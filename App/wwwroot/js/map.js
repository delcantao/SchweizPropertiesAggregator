// O MapLibre estará disponível globalmente como 'maplibregl'
const map = new maplibregl.Map({
  container: "map",

  style: "https://tiles.openfreemap.org/styles/liberty",

  center: [8.2275, 46.8182],
  zoom: 8,
});
map.on("error", (e) => {
  console.log("error debug", e);
});

map.on("moveend", async () => {
  const bounds = map.getBounds();

  const west = bounds.getWest();
  const south = bounds.getSouth();
  const east = bounds.getEast();
  const north = bounds.getNorth();

  refreshCards(west, south, east, north);
  refreshMap(west, south, east, north);
});

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

    const popupHtml = `
        <div class="popup">

            <img
                src="${properties.image}"
                style="width:100%; border-radius:12px;" />

            <h3>${properties.title}</h3>

            <p>
                CHF ${Number(properties.price).toLocaleString()}
            </p>

        </div>
    `;

    new maplibregl.Popup().setLngLat(coordinates).setHTML(popupHtml).addTo(map);
  });
}
