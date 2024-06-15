CREATE TABLE IF NOT EXISTS users (
    id UUID PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS airplanes (
    id UUID PRIMARY KEY,
    model VARCHAR(255) UNIQUE NOT NULL,
    weight VARCHAR(255) NOT NULL,
    manufacturer VARCHAR(255) NOT NULL
);
