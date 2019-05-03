-- Types
CREATE TYPE cache_status AS ENUM ('in_progress', 'failed', 'complete');

-- Tables
CREATE TABLE cache_meta (
    id text,
    created_at timestamp without time zone NOT NULL DEFAULT timezone('utc'::text, now()),
    updated_at timestamp without time zone,

    identifier text,
    cache_state cache_status NOT NULL,
    cache_failure jsonb,

    UNIQUE (identifier),
    PRIMARY KEY (id)
);

CREATE TABLE gamertag_replacements (
    id text,
    created_at timestamp without time zone NOT NULL DEFAULT timezone('utc'::text, now()),
    updated_at text,

    source_gamertag text NOT NULL,
    destination_xuid bigint NOT NULL,

    PRIMARY KEY (id)
);

CREATE TABLE service_records (
    id text,
    created_at timestamp without time zone NOT NULL DEFAULT timezone('utc'::text, now()),
    updated_at text,

    gamertag_ident text NOT NULL,
    gamertag text NOT NULL,
    emblem_url text NOT NULL,
    clan_name text,
    total_games int NOT NULL,
    total_kills int NOT NULL,
    total_deaths int NOT NULL,
    total_assists int NOT NULL,
    last_played timestamp without time zone NOT NULL,

    UNIQUE (gamertag),
    PRIMARY KEY (id)
);
