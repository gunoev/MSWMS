- repositories
    - DistributionRepository - базовый готов
    - DistributionDocument - готово
    - DistributionItem - +
    - DistributionScan - +

- создать сервис distribution
    сервис должен работать с Distribution entity.
    crud операции из DistributionRepository
    добавление и удаление Distribution.Documents
    изменение Distribution.Note

- создать сервис для работы с distribution document и item для получения информации из dcx wms
    - получение documents из wms - +
    - получение items для documents
    - каждый item имеет destination но destination нужно брать из order number из других таблиц
    
- создать сервис distribution scan
    - получение item и variant по баркоду из item cross reference wms
    - создание scan entity из request dto
    - проверка статуса scan
    - присвоение к item в зависимости от scan status
    - присвоение к document
    - удаление scan entity
    - фильтры по bincode lotnumber destination
    - обязательная проверка qty
    
- SignalR
    - hub для каждого distribution
    - отправление scan dto клиентам
    
- Controllers
    - DistributionController
        - POST /api/distributions/
        - GET /api/distributions/{id}
        - DELETE /api/distributions/{id}
        - PATCH /api/distributions/{id}/note
        
        - POST /api/distributions/{id}/documents request (List<string> number of documents)
        - GET /api/distributions/{id}/documents
        - GET /api/distributions/{id}/documents/{docId}
        - GET /api/distributions/{id}/documents/items
        - GET /api/distributions/{id}/documents/{docId}/items
        - GET /api/distributions/{id}/documents/{docId}/items/{itemId}
        - DELETE /api/distributions/{id}/documents/{docId}
        - PATCH /api/distributions/{id}/documents/{docId}
        
        - GET /api/distributions/{id}/scans
        - GET /api/distributions/{id}/scans/{scanId}
        - POST /api/distributions/{id}/scans
        - DELETE /api/distributions/{id}/scans/{scanId}
   
    
    
    
```bash
dotnet ef dbcontext scaffold "Server=192.168.51.13;Database=DCX-MS;User Id=pickapp;Password=pickapp123;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --table 'DCX-MS$Warehouse Activity Header' --table 'DCX-MS$Warehouse Activity Line' --table 'DCX-MS$Transfer Header' --table 'DCX-MS$Sales Header' --output-dir TempModels --context TempContext --force
```

---

### Detailed TODO Breakdown

#### 1. Distribution Service & Controller
- [ ] Перевести `DistributionController` на использование `IDistributionService` вместо прямого обращения к `AppDbContext`.
- [ ] Добавить в `DistributionController` эндпоинт `POST /api/Distribution/{id}/documents` для добавления документа.
- [ ] Добавить в `DistributionController` эндпоинт `DELETE /api/Distribution/{id}/documents/{docId}` для удаления документа.
- [ ] Добавить эндпоинт для обновления `Note` (Partial Update / PATCH или PUT).

#### 2. Интеграция с DCX WMS
- [ ] Перенести логику `LinesToDistributionItems` из `DcxDistributionService` в доступное место или сделать метод публичным.
- [ ] Реализовать метод для массовой загрузки `DistributionItem` из WMS на основе `DistributionDocument`.
- [ ] Реализовать метод `GetSalesDestinationLocationCode` в `DcxDistributionRepository` (сейчас `NotImplementedException`).
- [ ] Добавить поддержку "Sales Header" в `GetLocationCodeByOrderNumber` в `DcxDistributionService`.

#### 3. Сервис Сканирования Дистрибуций (Distribution Scan)
- [ ] Создать интерфейс `IDistributionScanService`.
- [ ] Создать реализацию `DistributionScanService`.
- [ ] Реализовать получение товара и варианта по штрихкоду через `Item Cross Reference` в WMS.
- [ ] Реализовать логику создания `DistributionScan` из DTO запроса.
- [ ] Реализовать проверку статуса сканирования (`Ok`, `Error`, `Excess`):
    - Сверка с ожидаемым количеством в `DistributionItem`.
    - Проверка принадлежности к документу.
- [ ] Реализовать метод удаления скана.
- [ ] Реализовать фильтрацию сканов по `BinCode`, `LotNumber`, `Destination`.
- [ ] Добавить обязательную валидацию `Qty` при обработке скана.

#### 4. SignalR и уведомления
- [ ] Добавить в `ScanHub` метод `JoinDistributionGroup(int distributionId)`.
- [ ] Добавить в `ScanHub` метод `LeaveDistributionGroup(int distributionId)`.
- [ ] Реализовать отправку события `distributionScanProcessed` всем клиентам в группе дистрибуции.
- [ ] Интегрировать вызов SignalR в `DistributionScanService` после каждого успешного сканирования.

#### 5. Тестирование и верификация
- [ ] Написать модульные тесты для `DistributionScanService` (логика статусов).
- [ ] Протестировать интеграцию с WMS (загрузка строк).
- [ ] Проверить работу SignalR уведомлений.




# Pipeline
    - Manager
        - Создание Distribution
        - Добавление документов в distribution (получение documents no из directedpick/headers)
        - Возможность удалить distribution или отдельные документы
        - Остальные возможности как у DistributionPicker
        
    - DistributionPicker
        - Просмотр списка distributions по date range
        - Выбор Distribution для работы (подключение к signalR hub)
        - сканирование
        - удаление scans (на каждом item кнопка для удаление скана, удаляется последний для этого user и item)
        - фильтры по lotNo по location (location выбор из списка) фильтры не должны сбрасываться (только вручную)
        - таблица scans как в Scanning (без box) (кнопка для навигации к item в другой таблице при клике)
       


















