# EncriptacionDeArchivos
Programa donde puedes seleccionar archivos a encriptar que luego seran enviados a un servidor donde seran encriptados

El cliente es una aplicacion de escritorio escrita en C# que te permite elegir un archivo o una carpeta (con archivos, no permite carpetas vacias), y enviarlas al servidor hecho en ExpressJS.

Antes de enviar los archivos, el cliente los encripta utilizando una llave simetrica, luego esta llave simetrica es encriptada con la llave asimetrica publica. Los bytes de la llave simetrica encriptada y el archivo encriptado generados son escritos en un nuevo archivo con extension .enc (de encriptado), que es el que se envia al servidor. El servidor al recibir este archivo (o archivos), lee los bytes de la llave simetrica encriptada y la desencripta con la llave asimetrica privada, al haberla desencriptado, ya se puede desencriptar el archivo original del lado del servidor y guardarlo en una carpeta.
