# redis-client

Client for wrapping redis.

## Options

This is the options structure.
``` json
{
	"host": "redis-server",
	"port": 6379,
	"database": 1,
	"password": "redis-password",
	"connectTimeout": 3000
}
```

## Usage

The following explains the basic usage of the package. There isn't much to it, so it isn't that hard to follow.

### Connecting

Just pass the options into the `RedisClient.connect()` async static function, this will return a class that is connected to your redis database.

```js
import RedisClient from 'redis-client';

const client = await RedisClient.connect({
	host: "redis-server",
	port: 6379,
	database: 1,
	password: "redis-password",
	connectTimeout: 3000
});
```

### Getting

To get a value from redis, simply do the following

```js
const value = await client.get('redis-key');

console.log(value);
```

### Setting

To set a value in your redis database, simple do the following. Note as this just wraps the `redis` npm client, you can pass anything into this function as you would if you were just calling the `SET` command. Docs [here](https://redis.io/commands/set).
```js
// Simple set
await client.set('redis-key', 'sensual-value');

// TTL set - this will remove the key after 60 seconds
await client.set('redis-key', '👊🏻💦🙈', 'EX', 60);
```
