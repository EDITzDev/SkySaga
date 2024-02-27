# Bundled Files
These folders have the files bundled with the SkySaga executable `SkySaga.exe`.

Older versions of the game bundled the files inside a `.pc` file which was loaded just like a resource pack, I've included the extracted files of the `.pc` in each folder.

## DataVersion.json
This file contains the `version` used by the client to calculate the `clientVersionNumber` for the `RPCClientConnected` packet.
The `clientVersionNumber` is calculated by first MD5 hashing ASCII `18` and then with the same buffer hashing the `version`.

## Entities.json
This file contains an array of entities, each entity has the following;

 - Name - Unique name, the server references these by hash (CRC32).
 - Components - Each component can have an array of `bindings` which are used by the client and server when they communicate.
 - Parameters - This is the list of parameters the entity supports and each parameter has a `syncindex` and an **optional** default `value`.

## GeoData.json
This file contains various definitions of game data such as;

- Biomes
- Emotes
- Jobs
- LootTables
- Recipes
- Voxels
- ...

## UIStyles.json
This file contains HTML styles used by the game UI.

## WWiseCatalogue.json
This file contains the following;

- Bank List - A list of the `.bnk` file paths used by the client.
- Catalogue - A list of all the sounds and their respective name, `id` and `bank` index.
