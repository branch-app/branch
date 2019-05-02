-- Types
CREATE TYPE cache_status AS ENUM ('in_progress', 'failed', 'complete');

-- Tables
CREATE TABLE service_records (
    id text,
    gamertag text,
    cache_state cache_status NOT NULL,
    cache_failure jsonb,
    created_at timestamp without time zone NOT NULL DEFAULT timezone('utc'::text, now()),
    updated_at timestamp without time zone,
    PRIMARY KEY (id),
    UNIQUE (gamertag)
);
