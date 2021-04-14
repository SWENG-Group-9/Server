# API Documentation

## `GET` `/api/current`
Returns the current number of people in the store in json format

Sample Output
```json
5
```

## `PUT` `/api/current/i`
Sets the current number of customers in the store to i

## `GET` `/api/max`
Returns the max number of people in the store in json format


## `PUT` `/api/max/i`
Sets the max customers in the store to i


## `PUT` `/api/door`

Overrides the lock mechanism and forces the door to either open or close (depending on what it was before issuing the command). Issuing this command again returns the door to automatic.


## `POST` `/api/devices/id/type`

Registers a new IoT Device under the id and type ("in","out","both") given in the request with the IoT Hub and returns the connection string needed to be entered into the device in json format

Sample Output
```json
HostName=sweng.azure-devices.net;DeviceId=id;SharedAccessKey=9+fS9USROYYFY5/cV/sxet+tCMEyh+xQV/rg/V6oOSE=
```

## `GET` `/api/devices`

Returns a list of all devices connected to the IoT hub and returns all relative data in JSON. The format is [[device name,id number,type of door, door status (true if locked, false if unlocked), door operation mode(true if manual, false if automatic)].

Sample Output
```json
[[az-c89346883a40,0,in,true,false] , [AZ-c89346886016,1,out,false,false]]
```


##`DELETE` `/api/devices/id`

Takes in a device id number and deletes it from the iot hub, as well as removing all references to it on the backend server.

## `GET` `/api/stats`
Returns json file with stats. (in same format of data.json in the front end branch)

## `GET` `/api/stats/id`
Returns json file with stats for specific date(YYYY-MM-DD). (in same format of data.json in the front end branch)

Sample Output
```json
{"2021-01-01":[{"time":"00:00",
    "value":21},
    {"time":"00:30",
    "value":68},
    {"time":"01:00",
    "value":63},
    {"time":"01:30",
    "value":21},
    {"time":"02:00",
    "value":17},
    {"time":"02:30",
    "value":64},
    {"time":"03:00",
    "value":32},
    {"time":"03:30",
    "value":11},
    {"time":"04:00",
    "value":62},
    {"time":"04:30",
    "value":15},
    {"time":"05:00",
    "value":20},
    {"time":"05:30",
    "value":69},
    {"time":"06:00",
    "value":59},
    {"time":"06:30",
    "value":47},
    {"time":"07:00",
    "value":28},
    {"time":"07:30",
    "value":54},
    {"time":"08:00",
    "value":69},
    {"time":"08:30",
    "value":24},
    {"time":"09:00",
    "value":11},
    {"time":"09:30",
    "value":22},
    {"time":"10:00",
    "value":61},
    {"time":"10:30",
    "value":33},
    {"time":"11:00",
    "value":25},
    {"time":"11:30",
    "value":65},
    {"time":"12:00",
    "value":68},
    {"time":"12:30",
    "value":41},
    {"time":"13:00",
    "value":39},
    {"time":"13:30",
    "value":10},
    {"time":"14:00",
    "value":17},
    {"time":"14:30",
    "value":66},
    {"time":"15:00",
    "value":26},
    {"time":"15:30",
    "value":51},
    {"time":"16:00",
    "value":34},
    {"time":"16:30",
    "value":39},
    {"time":"17:00",
    "value":31},
    {"time":"17:30",
    "value":13},
    {"time":"18:00",
    "value":63},
    {"time":"18:30",
    "value":14},
    {"time":"19:00",
    "value":30},
    {"time":"19:30",
    "value":30},
    {"time":"20:00",
    "value":11},
    {"time":"20:30",
    "value":57},
    {"time":"21:00",
    "value":60},
    {"time":"21:30",
    "value":51},
    {"time":"22:00",
    "value":29},
    {"time":"22:30",
    "value":69},
    {"time":"23:00",
    "value":20},
    {"time":"23:30",
    "value":15}]}
```
