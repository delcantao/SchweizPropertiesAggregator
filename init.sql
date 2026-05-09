CREATE EXTENSION IF NOT EXISTS postgis;

DROP TABLE IF EXISTS properties;

CREATE TABLE properties (
    id BIGINT PRIMARY KEY,
    
    title TEXT NOT NULL,
    
    price NUMERIC(12,2) NOT NULL,
    currency VARCHAR(10) NOT NULL,

    bedrooms INT NOT NULL,
    bathrooms INT NOT NULL,

    area NUMERIC(10,2) NOT NULL,

    city TEXT NOT NULL,
    address TEXT NOT NULL,

    latitude DOUBLE PRECISION NOT NULL,
    longitude DOUBLE PRECISION NOT NULL,

    location GEOGRAPHY(POINT, 4326) NOT NULL,

    images JSONB NOT NULL
);

CREATE INDEX idx_properties_location
ON properties
USING GIST(location);

INSERT INTO properties (
    id,
    title,
    price,
    currency,
    bedrooms,
    bathrooms,
    area,
    city,
    address,
    latitude,
    longitude,
    location,
    images
)
VALUES
(
    1,
    'Apartamento Moderno no Centro',
    1250000,
    'CHF',
    3,
    2,
    115,
    'Luzern',
    'Pilatusstrasse 12',
    47.050168,
    8.309307,
    ST_SetSRID(ST_MakePoint(8.309307, 47.050168), 4326)::geography,
    '[
        "https://picsum.photos/seed/apt1a/800/600",
        "https://picsum.photos/seed/apt1b/800/600",
        "https://picsum.photos/seed/apt1c/800/600"
    ]'::jsonb
),
(
    2,
    'Cobertura com Vista para o Lago',
    2850000,
    'CHF',
    4,
    3,
    240,
    'Luzern',
    'Haldenstrasse 8',
    47.048512,
    8.305921,
    ST_SetSRID(ST_MakePoint(8.305921, 47.048512), 4326)::geography,
    '[
        "https://picsum.photos/seed/apt2a/800/600",
        "https://picsum.photos/seed/apt2b/800/600",
        "https://picsum.photos/seed/apt2c/800/600"
    ]'::jsonb
),
(
    3,
    'Studio Compacto e Moderno',
    620000,
    'CHF',
    1,
    1,
    42,
    'Luzern',
    'Baselstrasse 55',
    47.053201,
    8.312110,
    ST_SetSRID(ST_MakePoint(8.312110, 47.053201), 4326)::geography,
    '[
        "https://picsum.photos/seed/apt3a/800/600",
        "https://picsum.photos/seed/apt3b/800/600"
    ]'::jsonb
),
(
    4,
    'Casa Familiar com Jardim',
    1980000,
    'CHF',
    5,
    3,
    310,
    'Luzern',
    'Meggerstrasse 22',
    47.044110,
    8.318994,
    ST_SetSRID(ST_MakePoint(8.318994, 47.044110), 4326)::geography,
    '[
        "https://picsum.photos/seed/apt4a/800/600",
        "https://picsum.photos/seed/apt4b/800/600",
        "https://picsum.photos/seed/apt4c/800/600"
    ]'::jsonb
),
(
    5,
    'Apartamento Minimalista',
    890000,
    'CHF',
    2,
    1,
    78,
    'Luzern',
    'Zürichstrasse 77',
    47.056442,
    8.301122,
    ST_SetSRID(ST_MakePoint(8.301122, 47.056442), 4326)::geography,
    '[
        "https://picsum.photos/seed/apt5a/800/600",
        "https://picsum.photos/seed/apt5b/800/600"
    ]'::jsonb
),
(
    6,
    'Loft Industrial Reformado',
    1540000,
    'CHF',
    2,
    2,
    145,
    'Luzern',
    'Industriestrasse 4',
    47.051221,
    8.296880,
    ST_SetSRID(ST_MakePoint(8.296880, 47.051221), 4326)::geography,
    '[
        "https://picsum.photos/seed/apt6a/800/600",
        "https://picsum.photos/seed/apt6b/800/600",
        "https://picsum.photos/seed/apt6c/800/600"
    ]'::jsonb
),
(
    7,
    'Casa Moderna nas Colinas',
    3200000,
    'CHF',
    6,
    4,
    420,
    'Luzern',
    'Sonnenbergweg 14',
    47.041550,
    8.325100,
    ST_SetSRID(ST_MakePoint(8.325100, 47.041550), 4326)::geography,
    '[
        "https://picsum.photos/seed/apt7a/800/600",
        "https://picsum.photos/seed/apt7b/800/600",
        "https://picsum.photos/seed/apt7c/800/600"
    ]'::jsonb
),
(
    8,
    'Apartamento Econômico',
    540000,
    'CHF',
    1,
    1,
    50,
    'Luzern',
    'Bernstrasse 19',
    47.057998,
    8.315778,
    ST_SetSRID(ST_MakePoint(8.315778, 47.057998), 4326)::geography,
    '[
        "https://picsum.photos/seed/apt8a/800/600",
        "https://picsum.photos/seed/apt8b/800/600"
    ]'::jsonb
),
(
    9,
    'Duplex Luxuoso',
    2650000,
    'CHF',
    4,
    3,
    260,
    'Luzern',
    'Nationalquai 3',
    47.047881,
    8.311500,
    ST_SetSRID(ST_MakePoint(8.311500, 47.047881), 4326)::geography,
    '[
        "https://picsum.photos/seed/apt9a/800/600",
        "https://picsum.photos/seed/apt9b/800/600",
        "https://picsum.photos/seed/apt9c/800/600"
    ]'::jsonb
),
(
    10,
    'Apartamento com Sacada Panorâmica',
    1320000,
    'CHF',
    3,
    2,
    122,
    'Luzern',
    'Seeburgstrasse 28',
    47.049712,
    8.320444,
    ST_SetSRID(ST_MakePoint(8.320444, 47.049712), 4326)::geography,
    '[
        "https://picsum.photos/seed/apt10a/800/600",
        "https://picsum.photos/seed/apt10b/800/600",
        "https://picsum.photos/seed/apt10c/800/600"
    ]'::jsonb
);


select * from properties;
