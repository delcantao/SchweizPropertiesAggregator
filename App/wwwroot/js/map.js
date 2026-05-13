// O MapLibre estará disponível globalmente como 'maplibregl'

const ROOT = "/wohnung"

const map = new maplibregl.Map({
  container: "map",
  style: "https://tiles.openfreemap.org/styles/liberty",
  center: [8.2275, 46.8182],
  zoom: 8,
});

// ─── Highlight card on map hover ─────────────────────────────────────────────

let highlightedCardId = null;
let highlightedMapPointId = null;
let allGeojsonFeatures = []; // cache for highlight lookups

const highlightedCardStyle = {
  outline: "3px solid #38bdf8",
  outlineOffset: "2px",
};

function clearHighlightedCard() {
  if (!highlightedCardId) return;
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
  if (!propertyId || highlightedCardId === propertyId) return;
  clearHighlightedCard();
  const card = document.querySelector(`[data-property-id="${propertyId}"]`);
  if (!card) return;
  card.style.outline = highlightedCardStyle.outline;
  card.style.outlineOffset = highlightedCardStyle.outlineOffset;
  highlightedCardId = propertyId;
}

function highlightMapPoint(propertyId) {
  const source = map.getSource("highlight");
  if (!source) return;
  const id = Number(propertyId);
  const feature = allGeojsonFeatures.find(f => f.properties.id === id);
  if (!feature) return;
  highlightedMapPointId = propertyId;
  source.setData({ type: "FeatureCollection", features: [feature] });
}

function clearHighlightedMapPoint() {
  const source = map.getSource("highlight");
  if (!source) return;
  highlightedMapPointId = null;
  source.setData({ type: "FeatureCollection", features: [] });
}

// ─── Filter helpers ───────────────────────────────────────────────────────────

function getFilters() {
  const params = {};

  // Deal type toggle buttons
  const activeDealtypeBtn = document.querySelector(
    '[data-filter="dealtype"].filter-active',
  );
  if (activeDealtypeBtn) {
    params.dealtype = activeDealtypeBtn.dataset.value;
  }

  // Bedrooms select
  const bedroomsEl = document.getElementById("filter-bedrooms");
  if (bedroomsEl && bedroomsEl.value !== "") {
    params.minBedrooms = bedroomsEl.value;
  }

  // Area slider
  const areaEl = document.getElementById("filter-area");
  if (areaEl && parseInt(areaEl.value) > 0) {
    params.minArea = areaEl.value;
  }

  // Price range
  const priceMinEl = document.getElementById("filter-price-min");
  const priceMaxEl = document.getElementById("filter-price-max");
  if (priceMinEl && priceMinEl.value !== "") {
    params.minPrice = priceMinEl.value;
  }
  if (priceMaxEl && priceMaxEl.value !== "") {
    params.maxPrice = priceMaxEl.value;
  }

  return params;
}

function buildQueryString(base) {
  const filters = getFilters();
  const all = { ...base, ...filters };
  return new URLSearchParams(all).toString();
}

// ─── Map refresh ─────────────────────────────────────────────────────────────

map.on("error", (e) => console.log("error debug", e));

map.on("moveend", () => refresh());

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
  const response = await fetch(`${ROOT}/api/properties/map`);
  const geojson = await response.json();
  allGeojsonFeatures = geojson.features ?? [];

  map.addSource("properties", {
    type: "geojson",
    data: geojson,
    cluster: true,
    clusterMaxZoom: 14,
    clusterRadius: 50,
  });

  // Separate non-clustered source used exclusively for the hover highlight
  map.addSource("highlight", {
    type: "geojson",
    data: { type: "FeatureCollection", features: [] },
  });

  addPropertyLayers();
});

function refreshCards(west, south, east, north) {
  const qs = buildQueryString({ west, south, east, north });
  htmx.ajax("GET", `${ROOT}/properties/cards?${qs}`, {
    target: "#cards",
    swap: "innerHTML",
  });
}

async function refreshMap(west, south, east, north) {
  const qs = buildQueryString({ west, south, east, north });
  const response = await fetch(`${ROOT}/api/properties/map?${qs}`);
  const geojson = await response.json();
  allGeojsonFeatures = geojson.features ?? [];
  const source = map.getSource("properties");
  if (!source) {
    console.error("Source not found", source);
    return;
  }
  source.setData(geojson);
}

// ─── Map layers ───────────────────────────────────────────────────────────────

function addPropertyLayers() {
  map.addLayer({
    id: "clusters",
    type: "circle",
    source: "properties",
    filter: ["has", "point_count"],
    paint: {
      "circle-stroke-width": 2,
      "circle-stroke-color": "#fff",
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

  map.addLayer({
    id: "highlighted-point",
    type: "circle",
    source: "highlight",
    paint: {
      "circle-color": "#f97316",
      "circle-radius": 14,
      "circle-stroke-width": 3,
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
            <img src="${images[0]}" style="width:100%; border-radius:12px;" />
            <h3>${properties.title}</h3>
            <p>CHF ${Number(properties.price).toLocaleString()}</p>
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

  // Wire card hover → highlight map point (event delegation on #cards container)
  document.addEventListener("mouseover", (e) => {
    const card = e.target.closest("[data-property-id]");
    if (!card) return;
    highlightMapPoint(card.dataset.propertyId);
  });

  document.addEventListener("mouseout", (e) => {
    const card = e.target.closest("[data-property-id]");
    if (!card) return;
    // Only clear if we're leaving the card entirely
    const relatedCard = e.relatedTarget?.closest("[data-property-id]");
    if (!relatedCard || relatedCard !== card) {
      clearHighlightedMapPoint();
    }
  });
}

// ─── Filter controls wiring ───────────────────────────────────────────────────

document.addEventListener("DOMContentLoaded", () => {
  // Deal type toggle buttons
  document.querySelectorAll('[data-filter="dealtype"]').forEach((btn) => {
    btn.addEventListener("click", () => {
      const isActive = btn.classList.contains("filter-active");

      // Deactivate all deal type buttons first
      document.querySelectorAll('[data-filter="dealtype"]').forEach((b) => {
        b.classList.remove(
          "filter-active",
          "border-blue-200",
          "bg-blue-50",
          "text-blue-700",
        );
        b.classList.add("border-stone-200", "bg-stone-50", "text-slate-700");
      });

      // Toggle: if it wasn't active, activate it
      if (!isActive) {
        btn.classList.add(
          "filter-active",
          "border-blue-200",
          "bg-blue-50",
          "text-blue-700",
        );
        btn.classList.remove(
          "border-stone-200",
          "bg-stone-50",
          "text-slate-700",
        );
      }

      if (map.loaded()) refresh();
    });
  });

  // Bedrooms select
  document.getElementById("filter-bedrooms")?.addEventListener("change", () => {
    if (map.loaded()) refresh();
  });

  // Area slider
  const areaSlider = document.getElementById("filter-area");
  const areaLabel = document.getElementById("filter-area-label");
  if (areaSlider) {
    areaSlider.addEventListener("input", () => {
      areaLabel.textContent = areaSlider.value;
    });
    areaSlider.addEventListener("change", () => {
      if (map.loaded()) refresh();
    });
  }

  // Price range inputs
  const debounceRefresh = debounce(() => { if (map.loaded()) refresh(); }, 500);
  document.getElementById("filter-price-min")?.addEventListener("input", debounceRefresh);
  document.getElementById("filter-price-max")?.addEventListener("input", debounceRefresh);
});

// ─── Search with Nominatim ────────────────────────────────────────────────────

function debounce(fn, delay) {
  let timer;
  return (...args) => {
    clearTimeout(timer);
    timer = setTimeout(() => fn(...args), delay);
  };
}

let geocodeController = null;

async function geocodeSearch(query) {
  if (!query.trim()) return;

  const spinner = document.getElementById("search-spinner");
  const noResult = document.getElementById("search-no-result");

  if (spinner) spinner.classList.remove("hidden");
  if (noResult) noResult.classList.add("hidden");

  // Abort any in-flight request
  if (geocodeController) geocodeController.abort();
  geocodeController = new AbortController();

  try {
    const url = `https://nominatim.openstreetmap.org/search?q=${encodeURIComponent(query)}&format=json&limit=1&countrycodes=ch`;
    const res = await fetch(url, {
      signal: geocodeController.signal,
      headers: { "Accept-Language": "pt-BR,pt;q=0.9,en;q=0.8" },
    });
    const data = await res.json();

    if (data.length === 0) {
      if (noResult) noResult.classList.remove("hidden");
      return;
    }

    const { lat, lon, boundingbox } = data[0];
    const latitude = parseFloat(lat);
    const longitude = parseFloat(lon);

    if (boundingbox && boundingbox.length === 4) {
      // Fly to bounding box for better zoom level
      map.fitBounds(
        [
          [parseFloat(boundingbox[2]), parseFloat(boundingbox[0])],
          [parseFloat(boundingbox[3]), parseFloat(boundingbox[1])],
        ],
        { padding: 60, maxZoom: 14, duration: 1200 },
      );
    } else {
      map.flyTo({ center: [longitude, latitude], zoom: 13, duration: 1200 });
    }
    // refresh() will be triggered by the map's moveend event
  } catch (err) {
    if (err.name === "AbortError") return; // Cancelled — ignore
    console.error("Geocode error", err);
  } finally {
    if (spinner) spinner.classList.add("hidden");
  }
}

const debouncedGeocode = debounce(geocodeSearch, 600);

document.addEventListener("DOMContentLoaded", () => {
  const searchInput = document.getElementById("search-input");
  if (!searchInput) return;

  searchInput.addEventListener("input", (e) => {
    const val = e.target.value;
    document.getElementById("search-no-result")?.classList.add("hidden");
    if (val.length >= 3) {
      debouncedGeocode(val);
    }
  });

  searchInput.addEventListener("keydown", (e) => {
    if (e.key === "Enter") {
      e.preventDefault();
      geocodeSearch(e.target.value);
    }
  });
});


 