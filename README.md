# Product-Extractor
Worker Service extractor de listado de productos de api hacia base de datos local

## Funcionalidad 
- Descarga de listado de productos de api.
- Recupera listado de base de datos haciendo uso de cache distribuida con redis.
- Compara si se encuentra producto en base de datos. Si lo encuentra lo actualiza y si no lo encuentra lo ingresa.

## Tecnologias y extras
- Base de datos Sql Server (uso de entity framework database first)
- Cache Redis (cache distribuida)
- Uso de excepciones custom.
- Seleccion del tiempo de delay del servicio a traves del archivo del configuracion.
