# API Surface

## WebMapCore.Controllers.AboutController

**Source:** `WebMapCore.Controllers\AboutController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| WhatsNews | POST | `-` | id: string [Query] | `Task<WhatsNewsItem[]>` |
| WhatsnewsOnline | POST | `-` | id: string [Query] | `Task<WhatsNewsItem[]>` |

## WebMapCore.Controllers.d

**Source:** `WebMapCore.Controllers\AccountController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Login | POST | `-` | m: LoginModel [Query] | `Task<IActionResult>` |
| LostPassword | POST | `-` | m: LostPasswordModel [Query] | `Task<IActionResult>` |
| PassChange | POST | `-` | m: ChangePasswordModel [Query] | `IActionResult` |
| Registration | POST | `-` | m: RegistrationModel [Query] | `Task<IActionResult>` |

## WebMapCore.Controllers.f

**Source:** `WebMapCore.Controllers\ApiMobileController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| InfoStage | POST | `-` | m: ApiMobileInfoStageModel [Body] | `IActionResult` |
| InfoStops | POST | `-` | m: ApiMobileInfoModel [Body] | `IActionResult` |
| InfoTrips | POST | `-` | m: ApiMobileInfoModel [Body] | `MobileStageResult` |
| InfoCP | POST | `-` | m: ApiMobileInfoModel [Body] | `IActionResult` |
| InfoRefills | POST | `-` | m: ApiMobileInfoModel [Body] | `IActionResult` |
| SubNodes | POST | `-` | m: ApiMobileSubNodesModel [Body]; mobileSubNodesHandler: IMobileSubNodesHandler [Services] | `Task<IActionResult>` |
| ValidateDemo | POST | `-` | - | `IActionResult` |
| GetLastInfo | POST | `-` | m: ApiMobileLastInfoModel [Body]; lastInfoLoader: ILastInfoLoader [Services] | `IActionResult` |
| Find | POST | `-` | m: ApiMobileFindModel [Body]; carFinder: ICarFinder [Services]; settingsContainer: IGlobalSettingsContainer [Services]; stageModulesBuilder: IStageModulesBuilder [Services]; helper: IMobileHelper [Services] | `MobileSubNodeItem[]` |

## WebMapCore.Controllers.c

**Base route:** `Api/UpdateToken`

**Source:** `WebMapCore.Controllers\ApiMobileLoginController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Login | POST | `-` | m: ApiMobileLoginModel [Body]; reportExporterFactory: IReportExporterFactory [Services] | `MobileLoginResult` |
| UpdateToken | POST | `-` | m: ApiMobileUpdateTokenModel [Body] | `IActionResult` |

## WebMapCore.Controllers.s

**Base route:** `Api/v2/GetUserData`

**Source:** `WebMapCore.Controllers\ApiMobileV2Controller.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Find | POST | `-` | m: ApiMobileV2FindModel [Body]; carFinder: ICarFinder [Services] | `IActionResult` |
| GetLastInfo | POST | `-` | m: ApiMobileLastInfoModel [Body]; lastInfoLoader: ILastInfoLoader [Services] | `IActionResult` |
| GetStages | POST | `-` | m: MobileV2RequestByIDModel [Body]; mobileSubNodesHandler: IMobileSubNodesHandlerV2 [Services] | `IActionResult` |
| GetStage | POST | `-` | m: ApiMobileV2StageModel [Body]; mobileGetStagesHandler: IMobileV2GetStagesHandler [Services] | `IActionResult` |
| GetTripSplitters | POST | `-` | m: MobileV2RequestByIDModel [Body] | `IActionResult` |
| InfoTrips | POST | `-` | m: ApiMobileV2TripsModel [Body] | `IActionResult` |
| InfoTrip | POST | `-` | m: ApiMobileV2TripModel [Body] | `IActionResult` |
| Login | POST | `-` | m: MobileLoginModel [Body]; reportExporterFactory: IReportExporterFactory [Services] | `Task<IActionResult>` |
| Login2FA | POST | `-` | m: MobileLogin2FAModel [Body]; reportExporterFactory: IReportExporterFactory [Services] | `Task<IActionResult>` |
| UpdateToken | POST | `-` | m: ApiMobileUpdateTokenModel [Body] | `IActionResult` |
| RefreshToken | POST | `-` | refreshTokenModel: RefreshTokenModel [Body] | `IActionResult` |
| LoadJournal | POST | `-` | m: MobileV2LoadJournalModel [Body]; addrBase: SettingsFindAddressCollection [Services] | `IActionResult` |
| Load | POST | `-` | m: MobileV2LoadMRules [Body]; systemSettings: ISystemSettings [Services] | `IActionResult` |
| GetReports | POST | `-` | m: MobileV2RequestByIDModel [Body]; reportFactory: IReportFactory [Services] | `object` |
| GetReport | POST | `-` | m: MobileV2GetReportModel [Body]; reportFactory: IReportFactory [Services] | `Task<IActionResult>` |
| SubNodes | POST | `-` | m: ApiMobileV2SubNodesModel [Body]; mobileSubNodesHandler: IMobileSubNodesHandlerV2 [Services] | `Task<IActionResult>` |
| GetUserData | POST | `-` | m: MobileV2GetUserDataModel [Body]; reportExporterFactory: IReportExporterFactory [Services] | `IActionResult` |

## WebMapCore.Controllers.AppsController

**Source:** `WebMapCore.Controllers\AppsController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Edit | POST | `-` | Data: [AsJson] AppEditModel [Query] | `IActionResult` |
| Delete | POST | `-` | id: int [Query] | `IActionResult` |

## WebMapCore.Controllers.c

**Source:** `WebMapCore.Controllers\CarController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Find | POST | `-` | id: int [Query]; value: string [Query]; virtualTreeId: int? [Query]; otherIDs: [AsJson] int[] [Query]; carFinder: ICarFinder [Services]; null: [AsJson] string[] properties = [Query] | `JNodeEx[]` |
| MessageSend | POST | `-` | id: int [Query]; type: int [Query]; ids: int[] [Query]; msg: string [Query] | `IActionResult` |

## WebMapCore.Controllers.a

**Source:** `WebMapCore.Controllers\CommandsController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Send | POST | `-` | m: SendModelPost [Query] | `Task<IActionResult>` |
| Delete | POST | `-` | id: int [Query] | `Task<IActionResult>` |

## WebMapCore.Controllers.CorsProxyController

**Base route:** `*`

**Source:** `WebMapCore.Controllers\CorsProxyController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Post | POST | `-` | - | `Task` |

## WebMapCore.Controllers.a

**Source:** `WebMapCore.Controllers\DDDFilesController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| ByDrivers | POST | `-` | id: int [Query]; sd: DateTime [Query]; ed: DateTime [Query]; drivers: int[] [Query] | `Task<JByDriverResult>` |
| ByDriversDetailed | POST | `-` | id: int [Query]; sd: DateTime [Query]; ed: DateTime [Query]; drivers: int[] [Query] | `Task<JByDriverResult>` |
| ByCars | POST | `-` | id: int [Query]; sd: DateTime [Query]; ed: DateTime [Query]; cars: string[] [Query] | `Task<JByCarsResult>` |
| ByCarsDetailed | POST | `-` | id: int [Query]; sd: DateTime [Query]; ed: DateTime [Query]; cars: string[] [Query] | `Task<JByCarsResult>` |
| CarList | POST | `-` | id: int [Query] | `Task<NameItemString[]>` |
| CardUpdateTimes | POST | `-` | id: int [Query]; drivers: int[] [Query] | `Task<JCardInfoItem[]>` |
| ByCarsHistory | POST | `-` | id: int [Query]; sd: DateTime [Query]; ed: DateTime [Query]; cars: string[] [Query] | `Task<IActionResult>` |
| CardDataPresence | POST | `-` | id: int [Query]; sd: DateTime [Query]; ed: DateTime [Query]; drivers: int[] [Query] | `Task<IActionResult>` |

## WebMapCore.Controllers.a

**Source:** `WebMapCore.Controllers\DriverController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Find | POST | `-` | idg: int [Query]; id: int [Query]; value: string [Query]; virtualTreeId: int? [Query]; null: [AsJson] string[] properties = [Query] | `JNodeEx[]` |

## WebMapCore.Controllers.FIDOController

**Source:** `WebMapCore.Controllers\FIDOController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Register | POST | `-` | parms: FIDORegisterParams [Body]; login: string [Query] | `unsafe IActionResult` |
| Login | POST | `-` | parms: FIDOAuthenticateParams [Body]; login: string [Query]; contextAccessor: IHttpContextAccessor [Services]; userFactory: IUserInfoFactory [Services] | `unsafe async Task<IActionResult>` |

## WebMapCore.Controllers.b

**Source:** `WebMapCore.Controllers\FilesController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Upload | POST | `-` | readInputFilesService: IReadInputFilesService [Services] | `Task<IActionResult>` |

## WebMapCore.Controllers.GeoController

**Source:** `WebMapCore.Controllers\GeoController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Find | POST | `-` | id: int [Query]; idg: int? [Query]; value: string [Query]; virtualTreeId: int? [Query]; null: [AsJson] string[] properties = [Query] | `Task<JNodeEx[]>` |

## WebMapCore.Controllers.MapNoteController

**Source:** `WebMapCore.Controllers\MapNoteController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Save | POST | `-` | Data: [AsJson] MapNoteSaveModel [Query] | `IActionResult` |
| Delete | POST | `-` | id: int [Query] | `IActionResult` |

## WebMapCore.Controllers.MessagesController

**Source:** `WebMapCore.Controllers\MessagesController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Edit | POST | `-` | item: DBOrgMessage [Query] | `IActionResult` |
| Delete | POST | `-` | id: int [Query] | `IActionResult` |

## WebMapCore.Controllers.a

**Source:** `WebMapCore.Controllers\NoteController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Save | POST | `-` | Data: [AsJson] NoteSaveModel [Query] | `IActionResult` |
| Delete | POST | `-` | id: int [Query] | `IActionResult` |

## WebMapCore.Controllers.RoutingController

**Source:** `WebMapCore.Controllers\RoutingController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Route | POST | `-` | points: [AsJson] RoutePoint[] [Query] | `Task<IActionResult>` |

## WebMapCore.Controllers.SchemaConvertController

**Base route:** `Api/SchemaConvert`

**Source:** `WebMapCore.Controllers\SchemaConvertController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| SchemaConvert | POST | `-` | - | `Task<IActionResult>` |

## WebMapCore.Controllers.b

**Base route:** `Api/SchemaUploadCompleted`

**Source:** `WebMapCore.Controllers\SchemaUploadController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| SchemaUploadMultipart | POST | `-` | - | `Task<IActionResult>` |

## WebMapCore.Controllers.c

**Source:** `WebMapCore.Controllers\SettingController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| GetMobileColumns | POST | `-` | id: int [Query]; type: int [Query]; stage: string [Query] | `MobileColumnsResult` |
| SetMobileColumns | POST | `-` | nodeId: int [Query]; stageName: string [Query]; columns: string[] [Query]; asInherited: int [Query]; forCurrentGroup: int [Query]; replaceInner: int [Query] | `IActionResult` |
| Save | POST | `-` | id: int [Query]; type: SettingType [Query]; value: string [Query]; idcar: int? [Query] | `IActionResult` |
| Load | POST | `-` | type: SettingType [Query]; def: string [Query] | `IActionResult` |
| GetPropertyCards | POST | `-` | id: int [Query] | `IActionResult` |
| EditPeriods | POST | `-` | Data: [AsJson] UserPeriodsSaveModel [Query] | `IActionResult` |
| SetColumnsWidth | POST | `-` | id: int [Query]; nodeId: int [Query]; grid: string [Query]; Dictionary<string: [AsJson] [Query]; columns: int> [Query]; gridColumnsHelper: IGridColumnsHelper [Services] | `IActionResult` |
| GetOrgColumnsWidth | POST | `-` | id: int [Query]; "": string stageName = [Query] | `Dictionary<string, int>` |
| SetFooterState | POST | `-` | id: int [Query]; nodeId: int [Query]; stageName: string [Query]; state: bool [Query] | `IActionResult` |
| SetTControlWorksColumns | POST | `-` | id: int [Query]; columnSet: string [Query] | `IActionResult` |
| GetTControlWorksColumns | POST | `-` | id: int [Query] | `IActionResult` |
| GroupingEdit | POST | `-` | Data: [AsJson] GroupSaveModel [Query] | `IActionResult` |
| GetGroupingStatus | POST | `-` | id: int [Query]; groupingHelper: IGroupingHelper [Services] | `JGroupViewItem[]` |
| SetTreeColumns | POST | `-` | id: int [Query]; elementType: ElementType [Query]; statuses: [AsJson] JStatusColumnInfo[] [Query]; props: [AsJson] JPropColumnInfo[] [Query]; treeColumnsHelper: ITreeColumnsHelper [Services]; partitionType: PartitionType [Query] | `JTreeColumns` |
| GetTreeColumns | POST | `-` | id: int [Query]; elementType: ElementType [Query]; treeColumnsHelper: ITreeColumnsHelper [Services]; (PartitionType: PartitionType partitionType = [Query] | `GetTreeColumnsResult` |
| SetTreeColumnsWidth | POST | `-` | id: int [Query]; tree: string [Query]; columns: [AsJson] SetTreeColumnsWidthModel [Query]; (PartitionType: PartitionType partitionType = [Query] | `IActionResult` |

## WebMapCore.Controllers.a

**Source:** `WebMapCore.Controllers\SourceController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Load | POST | `-` | id: int [Query] | `SourceViewItem[]` |
| Edit | POST | `-` | Data: [AsJson] SourceSaveModel [Query] | `IActionResult` |
| ChangeState | POST | `-` | id: int[] [Query]; state: bool [Query] | `IActionResult` |

## WebMapCore.Controllers.b

**Source:** `WebMapCore.Controllers\TokensController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Delete | POST | `-` | id: int[] [Query] | `IActionResult` |
| Generate | POST | `-` | m: MakeTokenModel [Query] | `IActionResult` |

## WebMapCore.Controllers.g

**Source:** `WebMapCore.Controllers\TrackController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Positions | POST | `-` | id: long [Query]; type: JSTreeNodeType [Query]; idgeo: int? [Query]; gtype: int? [Query]; idcars: int[] [Query]; virtualTreeId: int? [Query]; lastPositionsLoader: ILastPositionsLoader [Services] | `Task<IActionResult>` |

## WebMapCore.Controllers.e

**Source:** `WebMapCore.Controllers\VirtualTreeController.cs`

| Method | HTTP | Route | Parameters | Returns |
|--------|------|-------|------------|---------|
| Load | POST | `-` | id: int [Query] | `TreeItem[]` |
| LoadGF | POST | `-` | id: int [Query] | `TreeItem[]` |
| Edit | POST | `-` | id: int [Query]; idorg: int [Query]; name: string [Query]; icon: string [Query]; target: [AsJson] VirtualTreeSaveItemModel[] [Query] | `IActionResult` |
| EditGF | POST | `-` | id: int [Query]; idorg: int [Query]; name: string [Query]; icon: string [Query]; target: [AsJson] VirtualTreeSaveItemModel[] [Query] | `IActionResult` |
| Delete | POST | `-` | id: int [Query] | `IActionResult` |
| DeleteGF | POST | `-` | id: int [Query] | `IActionResult` |


