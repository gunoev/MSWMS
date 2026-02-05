# API Endpoints

Base and conventions:
- Base URL: `https://0.0.0.0:5262`
- All routes are under `/api/...`
- Date parameters are strings parsed by `DateTime.Parse`.
- Pagination defaults: `page=1`, `pageSize=10` unless noted.
- Max `pageSize` for orders is 50.

SignalR:
- Hub URL: `/api/scanhub`
- Client methods (server emits): `scanProcessed`, `scanDeleted`, `boxDeleted`, `loadEventProcessed`, `loadEventDeleted`, `messageReceived`
- Group names: `Order_{orderId}`, `Shipment_{shipmentId}`

## AuthController (`/api/Auth`)

**POST** `/api/Auth/login`
- Auth: Anonymous
- Request body: `LoginModel` with `Username`, `Password`
- Response 200: `{ Token, User { Id, Name, Username, Email, Roles[] } }`
- Errors: 400 invalid model, 401 wrong credentials

**POST** `/api/Auth/change-password`
- Auth: `RequireAdmin`
- Parameters: `userId` (int), `newPassword` (string) as query or form
- Response 200: `{ Message }`
- Errors: 400 user not found

**POST** `/api/Auth/register`
- Auth: Anonymous
- Request body: `RegisterModel`
- Response 200: `{ Token, User { Id, Name, Username, Email, Roles[] } }`
- Errors: 400 invalid model or user already exists

**GET** `/api/Auth/me`
- Auth: Any authenticated user
- Response 200: `{ Username, Email, Name, Roles[] }`

**POST** `/api/Auth/logout`
- Auth: Allows anonymous and authenticated
- Response 200: `{ Message }`
- Notes: Signs out cookie auth if present

## AdminController (`/api/Admin`)

**GET** `/api/Admin/test`
- Auth: Anonymous
- Response 200: plain string

**GET** `/api/Admin/users`
- Auth: `RequireAdmin`
- Response 200: array of `{ Id, Username, Name, Email, Status, Roles[] }`

## BoxController (`/api/Box`)

**GET** `/api/Box/order/{orderId}`
- Auth: Not specified (accessible if auth is not enforced globally)
- Response 200: array of `BoxDto`

**GET** `/api/Box/{id}`
- Auth: Not specified
- Response 200: `BoxDto`
- Errors: 404 not found

**PUT** `/api/Box/{id}`
- Auth: `RequireAdmin`
- Request body: `Box` entity
- Response 204 No Content

**POST** `/api/Box`
- Auth: `RequireAdmin`
- Request body: `Box` entity
- Response 201 Created

**DELETE** `/api/Box/{id}`
- Auth: `RequirePicker`
- Response 204 No Content
- Errors: 400 not allowed, 404 not found
- Notes: Also emits `boxDeleted` over SignalR

## ItemController (`/api/Item`)

**GET** `/api/Item/order/{orderId}`
- Auth: Any authenticated user
- Response 200: array of `ItemDto`

**GET** `/api/Item/remaining/{barcode}`
- Auth: Any authenticated user
- Response 200: array of remaining quantities by order

**GET** `/api/Item/{id}`
- Auth: Any authenticated user
- Response 200: `ItemDto`
- Errors: 404 not found

**PUT** `/api/Item/{id}`
- Auth: `RequireAdmin`
- Request body: `Item` entity
- Response 204 No Content

**POST** `/api/Item`
- Auth: `RequireAdmin`
- Request body: `Item` entity
- Response 201 Created

**DELETE** `/api/Item/{id}`
- Auth: `RequireAdmin`
- Response 204 No Content

## ItemInfoController (`/api/ItemInfo`)

**GET** `/api/ItemInfo/barcode/{barcode}`
- Auth: Any authenticated user
- Response 200: `ItemInfo`
- Errors: 404 not found

**GET** `/api/ItemInfo/barcode-like/{barcode}`
- Auth: Any authenticated user
- Response 200: array of `ItemInfo` (max 10)

**GET** `/api/ItemInfo/item-number/{itemNumber}`
- Auth: Any authenticated user
- Response 200: array of `ItemInfo`

**GET** `/api/ItemInfo/{id}`
- Auth: Any authenticated user
- Response 200: `ItemInfo`
- Errors: 404 not found

**PUT** `/api/ItemInfo/{id}`
- Auth: `RequireAdmin`
- Request body: `ItemInfo` entity
- Response 204 No Content

**POST** `/api/ItemInfo`
- Auth: `RequireAdmin`
- Request body: `ItemInfo` entity
- Response 201 Created

**DELETE** `/api/ItemInfo/{id}`
- Auth: `RequireAdmin`
- Response 204 No Content

**POST** `/api/ItemInfo/is-exists`
- Auth: Not specified
- Request body: array of barcodes
- Response 200: map `{ barcode: bool }`

**POST** `/api/ItemInfo/upload-csv`
- Auth: `RequireAdmin`
- Request: `multipart/form-data` with file field `file`
- Response 200: integer `200`
- Limits: 300 MB

## LocationController (`/api/Location`)

**GET** `/api/Location`
- Auth: Any authenticated user
- Query: `page`, `pageSize`
- Response 200: `LocationList`

**GET** `/api/Location/{id}`
- Auth: Any authenticated user
- Response 200: `Location`
- Errors: 404 not found

**PUT** `/api/Location/{id}`
- Auth: `RequireManager`
- Request body: `Location` entity
- Response 204 No Content

**POST** `/api/Location`
- Auth: `RequireManager`
- Request body: `Location` entity
- Response 201 Created

**GET** `/api/Location/code-exists?code=...`
- Auth: Not specified
- Response 200: boolean

**DELETE** `/api/Location/{id}`
- Auth: `RequireManager`
- Response 204 No Content
- Errors: 400 when `id=0`, 404 not found

## OrderController (`/api/Order`)

**GET** `/api/Order`
- Auth: Any authenticated user
- Query: `page`, `pageSize`
- Response 200: `OrderList`
- Errors: 400 if `pageSize > 50`

**GET** `/api/Order/filter`
- Auth: Any authenticated user
- Query: `page`, `pageSize`, `shipmentId`, `transferOrderNumber`, `transferShipmentNumber`, `origin`, `destination`, `status`, `type`, `priority`, `createdFrom`, `createdTo`, `sortBy`, `descending`
- Response 200: `OrderList`

**GET** `/api/Order/details/{id}`
- Auth: `RequirePicker`
- Response 200: `Order` with items and destination

**GET** `/api/Order/{id}`
- Auth: `RequireAdmin`
- Response 200: `Order`

**PUT** `/api/Order/{id}`
- Auth: `RequireManager`
- Request body: `CreateOrderRequest`
- Response: `403 Forbid` (currently not implemented)

**POST** `/api/Order/reset/{orderId}`
- Auth: `RequireManager`
- Response 200: empty
- Errors: 400 if order not found or has shipments

**POST** `/api/Order`
- Auth: `RequireManager`
- Request body: `CreateOrderRequest` (user id is replaced by current user)
- Response 201 Created
- Errors: 400 if duplicate shipment id or transfer shipment number

**DELETE** `/api/Order/{id}`
- Auth: `RequireAdmin`
- Response 204 No Content

**GET** `/api/Order/create/{ts}`
- Auth: `RequireAdmin`
- Response 200: `Order` created from external source

**GET** `/api/Order/get-id/{no}`
- Auth: `RequirePicker`
- Response 200: `int` order id
- Errors: 404 if order not found and cannot be created
- Notes: If `no` starts with `db-`, it is resolved to shipment number first

**POST** `/api/Order/update-remark/{id}?remark=...`
- Auth: `RequireManagerOrDispatcher`
- Response 200: empty

**GET** `/api/Order/shipment-id-exists?shipmentId=...`
- Auth: Not specified
- Response 200: boolean

**GET** `/api/Order/order-number-exists?transferOrderNumber=...`
- Auth: Not specified
- Response 200: boolean

**GET** `/api/Order/shipment-number-exists?transferShipmentNumber=...`
- Auth: Not specified
- Response 200: boolean

**POST** `/api/Order/upload-excel`
- Auth: `RequireManager`
- Request: `multipart/form-data` with file field `file`
- Response 200: `ExcelParsedOrder`

## SalesPriceController (`/api/SalesPrice`)

**GET** `/api/SalesPrice`
- Auth: Not specified
- Response 200: list of sales prices with `StartingDate = today`

**GET** `/api/SalesPrice/default`
- Auth: Not specified
- Response 200: list of default sales prices

## SalesShipmentHeader (`/api/SalesShipmentHeader`)

**GET** `/api/SalesShipmentHeader/page={page}?shipToName=...`
- Auth: Not specified
- Response 200: list of recent sales shipment headers
- Notes: Filters by last month and `LocationCode = W01`

**GET** `/api/SalesShipmentHeader/details/{no}`
- Auth: Not specified
- Response 200: sales shipment header with lines
- Errors: 404 not found

## ScanController (`/api/Scan`)

**GET** `/api/Scan/{id}`
- Auth: `RequirePicker`
- Response 200: `ScanDto`

**GET** `/api/Scan/count/{fromDate}/to/{toDate}`
- Auth: `RequirePicker`
- Response 200: integer count for current user

**GET** `/api/Scan/statistic/{fromDate}/to/{toDate}`
- Auth: `RequireManager`
- Response 200: per-day statistics with per-user totals

**GET** `/api/Scan/order/{id}`
- Auth: `RequirePicker`
- Response 200: array of `ScanDto`

**DELETE** `/api/Scan?id=...`
- Auth: `RequirePicker`
- Response 204 No Content
- Errors: 400 not allowed, 404 not found
- Notes: Also emits `scanDeleted` over SignalR

**DELETE** `/api/Scan/delete-many/{orderId}?ids=1&ids=2`
- Auth: `RequirePicker`
- Response 204 No Content
- Errors: 400 user not found, 404 scans not found
- Notes: Removes empty boxes and emits `scanDeleted` and `boxDeleted`

**POST** `/api/Scan`
- Auth: `RequirePicker`
- Request body: `ScanRequest` (user id is replaced by current user)
- Response 200: `ScanResponse`
- Notes: Emits `scanProcessed` over SignalR

## ShipmentController (`/api/Shipment`)

**GET** `/api/Shipment?from=...&to=...`
- Auth: `RequireLoadingOperator`
- Response 200: array of `ShipmentDto`
- Errors: 400 if date range > 7 days

**GET** `/api/Shipment/{id}`
- Auth: `RequireLoadingOperator`
- Response 200: `Shipment`

**GET** `/api/Shipment/shipment-stats/{shipmentId}`
- Auth: `RequireLoadingOperator`
- Response 200: `{ TotalBoxes, TotalLoadedBoxes, LoadedBoxes }`

**PUT** `/api/Shipment/{id}`
- Auth: `RequireDispatcher`
- Request body: `CreateShipmentRequest`
- Response 200: empty
- Errors: 404 for location or orders not found

**POST** `/api/Shipment`
- Auth: `RequireDispatcher`
- Request body: `CreateShipmentRequest`
- Response 200: empty
- Errors: 404 for location or orders not found

**DELETE** `/api/Shipment/{id}`
- Auth: `RequireDispatcher`
- Response 204 No Content

## ShipmentEventController (`/api/ShipmentEvent`)

**GET** `/api/ShipmentEvent`
- Auth: `RequireLoadingOperator`
- Response 200: array of `ShipmentEvent`

**GET** `/api/ShipmentEvent/Shipment/{shipmentId}`
- Auth: `RequireLoadingOperator`
- Response 200: array of `ShipmentEventDto`

**GET** `/api/ShipmentEvent/{id}`
- Auth: `RequireLoadingOperator`
- Response 200: `ShipmentEvent`
- Errors: 404 not found

**PUT** `/api/ShipmentEvent/{id}`
- Auth: `RequireAdmin`
- Request body: `ShipmentEvent` entity
- Response 204 No Content

**POST** `/api/ShipmentEvent/load`
- Auth: `RequireLoadingOperator`
- Request body: `ShipmentEventRequest` with `Action = Load`
- Response 200: empty
- Errors: 400 invalid action or shipment id, 404 shipment not found
- Notes: Emits `loadEventProcessed` over SignalR

**POST** `/api/ShipmentEvent`
- Auth: `RequireLoadingOperator`
- Request body: `ShipmentEvent` entity
- Response 201 Created

**DELETE** `/api/ShipmentEvent/{id}`
- Auth: `RequireLoadingOperator`
- Response 204 No Content
- Notes: Emits `loadEventDeleted` over SignalR

## TransferShipmentHeader (`/api/TransferShipmentHeader`)

**GET** `/api/TransferShipmentHeader/page={page}?transferToName=...`
- Auth: Not specified
- Response 200: list of recent transfer shipment headers
- Notes: Filters by last month and `TransferFromCode = W01`

**GET** `/api/TransferShipmentHeader/details/{no}`
- Auth: Not specified
- Response 200: transfer shipment header with lines
- Errors: 404 not found

## UserController (`/api/User`)

**GET** `/api/User`
- Auth: `RequireManager`
- Response 200: array of `{ Id, Name, Username, Email, Roles[], Status, Location }`

**GET** `/api/User/{id}`
- Auth: `RequireAdmin`
- Response 200: `User`
- Errors: 404 not found

**PUT** `/api/User/{id}`
- Auth: `RequireAdmin`
- Request body: `User` entity
- Response 204 No Content

**POST** `/api/User`
- Auth: `RequireAdmin`
- Request body: `User` entity
- Response 201 Created

**POST** `/api/User/add-user`
- Auth: `RequireManager`
- Request body: `RegisterModel`
- Response 200: `{ Token, User { Id, Name, Username, Email, Roles[] } }`
- Errors: 403 when non-admin tries to add admin role

**DELETE** `/api/User/{id}`
- Auth: `RequireAdmin`
- Response 204 No Content
