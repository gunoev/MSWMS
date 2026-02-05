# Services

This file documents internal service classes in `Services/` and their public methods.

## AuthService

- `Login(LoginModel model)`
- Validates user, password hash, and user status.
- Returns `AuthResult` with `Token` and `User` on success.

- `Register(RegisterModel model)`
- Validates uniqueness for username and email.
- Assigns roles (defaults to `Observer`) and location (default location if `LocationId = 0`).
- Returns `AuthResult` with `Token` and `User` on success.

- `ChangePassword(int userId, string newPassword)`
- Replaces user password hash.
- Throws `InvalidOperationException` if user not found.

## BoxService

- `AddBox(Box box)`
- Adds a new box and saves changes.

- `UpdateBox(Box box)`
- Updates an existing box.

- `DeleteBox(Box box)`
- Removes a box and saves changes.

- `EntityToDto(Box box)`
- Maps `Box` to `BoxDto` and fills `HasShipmentEvents`.

- `GetBoxByNumberAndOrder(int boxNumber, int orderId)`
- Returns the box for a given order and box number with user and scans.

- `GetBoxById(int id)`
- Returns a box by id.

## OrderService

- `GetByIdAsync(int id)`
- Returns an order by id using repository.

- `CreateOrder(string shipmentNumber, string shippingId = "")`
- Creates a sales or transfer order based on shipment number (`SS` or `TS`).

- `LocallyExists(string shipmentNumber)`
- Returns existing order id by transfer shipment number.

- `GetShipmentNumberByShippingId(string shippingId)`
- Reads a shipment number from a network Excel file path.

- `UpdateOrderStatus(int orderId)`
- Sets order status to `New`, `InProgress`, or `Collected` based on scanned quantity.
- Updates `LastChangeDateTime` and `CollectedDateTime` when applicable.

## ReportService

- Currently no public methods.

## ScanService

- `ProcessScan(ScanRequest request)`
- Resolves item, order, box, and user.
- Creates a scan with status `Ok`, `Excess`, `NotFound`, or `Error`.
- Adds scan to order and updates order status.
- Returns `ScanResponse` with `Scan`, `Box`, and optional `Item`.

- `GetScannedQuantity(Item item, Order order)`
- Returns count of scans for item in order with status `Ok`.

- `GetItemByBarcodeAndOrder(string barcode, int orderId)`
- Resolves item by barcode within the order.

- `AddScanToOrder(Scan scan, Order order)`
- Adds scan, saves changes, updates order status.

- `DeleteScanFromOrder(Scan scan, Order order)`
- Removes scan, saves changes, updates order status.

## UserService

- `GetUserByIdAsync(int userId)`
- Returns a user by id.

## Interfaces

- `IAuthService`: `Login`, `Register`, `ChangePassword`
- `IScanService`: `ProcessScan`, `GetItemByBarcodeAndOrder`, `AddScanToOrder`, `DeleteScanFromOrder`, `GetScannedQuantity`
