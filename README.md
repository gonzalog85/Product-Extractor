# Product-Extractor
Worker Service para extraccion de listado de productos de api hacia base de datos local

## Funcionalidad 
- Descarga de listado de productos de api
- Recupera listado de base de datos haciendo uso de cache distribuida con redis
- Compara si se encuentra producto en base de datos. Si lo encuentra lo actualiza y si no lo encuentra lo ingresa a la base de datos

## Tecnologias y extras
- Base de datos Sql Server (uso de entity framework - creacion automatia de tabla al iniciar)
- Cache Redis (cache distribuida)
- Uso de excepciones custom
- Selección del tiempo de delay del servicio a traves del archivo del configuración

## Ejecucion
- Configurar en appSettings.json la cadena de conección sqlServer, cadena de conección redis
- Configurar en appSettings.json url api y apikey
- configurar en appSettings.json delay en minutos
