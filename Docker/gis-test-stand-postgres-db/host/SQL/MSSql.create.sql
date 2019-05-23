/****** Pre-script START ******/
CREATE TABLE [LayerMetadata] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NOT NULL,

	 [Description] VARCHAR(MAX)  NULL,

	 [KeyWords] VARCHAR(MAX)  NULL,

	 [Type] VARCHAR(255)  NOT NULL,

	 [Settings] VARCHAR(MAX)  NULL,

	 [Scale] INT  NULL,

	 [CoordinateReferenceSystem] VARCHAR(255)  NULL,

	 [BoundingBox] geography  NULL,

	 [CreateTime] DATETIME  NULL,

	 [Creator] VARCHAR(255)  NULL,

	 [EditTime] DATETIME  NULL,

	 [Editor] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [DataLink] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ClearWithoutLink] BIT  NULL,

	 [LayerTable] VARCHAR(255)  NOT NULL,

	 [CreateObject] BIT  NULL,

	 [MapObjectSetting] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [LinkMetadata] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [AllowShow] BIT  NULL,

	 [CreateTime] DATETIME  NULL,

	 [Creator] VARCHAR(255)  NULL,

	 [EditTime] DATETIME  NULL,

	 [Editor] VARCHAR(255)  NULL,

	 [MapObjectSetting] UNIQUEIDENTIFIER  NOT NULL,

	 [Layer] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [MapLayer] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NULL,

	 [Description] VARCHAR(MAX)  NULL,

	 [KeyWords] VARCHAR(MAX)  NULL,

	 [Index] INT  NULL,

	 [Visibility] BIT  NULL,

	 [Type] VARCHAR(255)  NOT NULL,

	 [Settings] VARCHAR(MAX)  NULL,

	 [Scale] INT  NULL,

	 [CoordinateReferenceSystem] VARCHAR(255)  NULL,

	 [BoundingBox] geography  NULL,

	 [CreateTime] DATETIME  NULL,

	 [Creator] VARCHAR(255)  NULL,

	 [EditTime] DATETIME  NULL,

	 [Editor] VARCHAR(255)  NULL,

	 [Parent] UNIQUEIDENTIFIER  NULL,

	 [Map] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ParameterMetadata] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ObjectField] VARCHAR(255)  NULL,

	 [LayerField] VARCHAR(255)  NULL,

	 [Expression] VARCHAR(255)  NULL,

	 [QueryKey] VARCHAR(255)  NULL,

	 [LinkField] BIT  NULL,

	 [CreateTime] DATETIME  NULL,

	 [Creator] VARCHAR(255)  NULL,

	 [EditTime] DATETIME  NULL,

	 [Editor] VARCHAR(255)  NULL,

	 [LayerLink] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [CswConnection] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NOT NULL,

	 [Url] VARCHAR(255)  NOT NULL,

	 [CreateTime] DATETIME  NULL,

	 [Creator] VARCHAR(255)  NULL,

	 [EditTime] DATETIME  NULL,

	 [Editor] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [MapObjectSetting] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [TypeName] VARCHAR(255)  NULL,

	 [ListForm] VARCHAR(255)  NULL,

	 [EditForm] VARCHAR(255)  NULL,

	 [Title] VARCHAR(255)  NULL,

	 [DefaultMap] UNIQUEIDENTIFIER  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Map] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NOT NULL,

	 [Description] VARCHAR(MAX)  NULL,

	 [KeyWords] VARCHAR(MAX)  NULL,

	 [Lat] FLOAT  NULL,

	 [Lng] FLOAT  NULL,

	 [Zoom] FLOAT  NULL,

	 [Public] BIT  NOT NULL,

	 [Scale] INT  NULL,

	 [CoordinateReferenceSystem] VARCHAR(255)  NULL,

	 [BoundingBox] geography  NULL,

	 [CreateTime] DATETIME  NULL,

	 [Creator] VARCHAR(255)  NULL,

	 [EditTime] DATETIME  NULL,

	 [Editor] VARCHAR(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [LinkParameter] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ObjectField] VARCHAR(255)  NULL,

	 [LayerField] VARCHAR(255)  NULL,

	 [Expression] VARCHAR(255)  NULL,

	 [QueryKey] VARCHAR(255)  NULL,

	 [LinkField] BIT  NULL,

	 [LayerLink] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [DataLinkParameter] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [ObjectField] VARCHAR(255)  NULL,

	 [LayerField] VARCHAR(255)  NULL,

	 [Expression] VARCHAR(255)  NULL,

	 [LinkField] BIT  NULL,

	 [Link] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [LayerLink] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [AllowShow] BIT  NULL,

	 [MapObjectSetting] UNIQUEIDENTIFIER  NOT NULL,

	 [Layer] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMNETLOCKDATA] (

	 [LockKey] VARCHAR(300)  NOT NULL,

	 [UserName] VARCHAR(300)  NOT NULL,

	 [LockDate] DATETIME  NULL,

	 PRIMARY KEY ([LockKey]))


CREATE TABLE [STORMSETTINGS] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Module] varchar(1000)  NULL,

	 [Name] varchar(255)  NULL,

	 [Value] text  NULL,

	 [User] varchar(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMAdvLimit] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [User] varchar(255)  NULL,

	 [Published] bit  NULL,

	 [Module] varchar(255)  NULL,

	 [Name] varchar(255)  NULL,

	 [Value] text  NULL,

	 [HotKeyData] int  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMFILTERSETTING] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Name] varchar(255)  NOT NULL,

	 [DataObjectView] varchar(255)  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMWEBSEARCH] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Name] varchar(255)  NOT NULL,

	 [Order] INT  NOT NULL,

	 [PresentView] varchar(255)  NOT NULL,

	 [DetailedView] varchar(255)  NOT NULL,

	 [FilterSetting_m0] uniqueidentifier  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMFILTERDETAIL] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Caption] varchar(255)  NOT NULL,

	 [DataObjectView] varchar(255)  NOT NULL,

	 [ConnectMasterProp] varchar(255)  NOT NULL,

	 [OwnerConnectProp] varchar(255)  NULL,

	 [FilterSetting_m0] uniqueidentifier  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMFILTERLOOKUP] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [DataObjectType] varchar(255)  NOT NULL,

	 [Container] varchar(255)  NULL,

	 [ContainerTag] varchar(255)  NULL,

	 [FieldsToView] varchar(255)  NULL,

	 [FilterSetting_m0] uniqueidentifier  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [UserSetting] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [AppName] varchar(256)  NULL,

	 [UserName] varchar(512)  NULL,

	 [UserGuid] uniqueidentifier  NULL,

	 [ModuleName] varchar(1024)  NULL,

	 [ModuleGuid] uniqueidentifier  NULL,

	 [SettName] varchar(256)  NULL,

	 [SettGuid] uniqueidentifier  NULL,

	 [SettLastAccessTime] DATETIME  NULL,

	 [StrVal] varchar(256)  NULL,

	 [TxtVal] varchar(max)  NULL,

	 [IntVal] int  NULL,

	 [BoolVal] bit  NULL,

	 [GuidVal] uniqueidentifier  NULL,

	 [DecimalVal] decimal(20,10)  NULL,

	 [DateTimeVal] DATETIME  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ApplicationLog] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Category] varchar(64)  NULL,

	 [EventId] INT  NULL,

	 [Priority] INT  NULL,

	 [Severity] varchar(32)  NULL,

	 [Title] varchar(256)  NULL,

	 [Timestamp] DATETIME  NULL,

	 [MachineName] varchar(32)  NULL,

	 [AppDomainName] varchar(512)  NULL,

	 [ProcessId] varchar(256)  NULL,

	 [ProcessName] varchar(512)  NULL,

	 [ThreadName] varchar(512)  NULL,

	 [Win32ThreadId] varchar(128)  NULL,

	 [Message] varchar(2500)  NULL,

	 [FormattedMessage] varchar(max)  NULL,

	 PRIMARY KEY ([primaryKey]))




 ALTER TABLE [DataLink] ADD CONSTRAINT [DataLink_FMapObjectSetting_0] FOREIGN KEY ([MapObjectSetting]) REFERENCES [MapObjectSetting]
CREATE INDEX DataLink_IMapObjectSetting on [DataLink] ([MapObjectSetting])

 ALTER TABLE [LinkMetadata] ADD CONSTRAINT [LinkMetadata_FMapObjectSetting_0] FOREIGN KEY ([MapObjectSetting]) REFERENCES [MapObjectSetting]
CREATE INDEX LinkMetadata_IMapObjectSetting on [LinkMetadata] ([MapObjectSetting])

 ALTER TABLE [LinkMetadata] ADD CONSTRAINT [LinkMetadata_FLayerMetadata_0] FOREIGN KEY ([Layer]) REFERENCES [LayerMetadata]
CREATE INDEX LinkMetadata_ILayer on [LinkMetadata] ([Layer])

 ALTER TABLE [MapLayer] ADD CONSTRAINT [MapLayer_FMapLayer_0] FOREIGN KEY ([Parent]) REFERENCES [MapLayer]
CREATE INDEX MapLayer_IParent on [MapLayer] ([Parent])

 ALTER TABLE [MapLayer] ADD CONSTRAINT [MapLayer_FMap_0] FOREIGN KEY ([Map]) REFERENCES [Map]
CREATE INDEX MapLayer_IMap on [MapLayer] ([Map])

 ALTER TABLE [ParameterMetadata] ADD CONSTRAINT [ParameterMetadata_FLinkMetadata_0] FOREIGN KEY ([LayerLink]) REFERENCES [LinkMetadata]
CREATE INDEX ParameterMetadata_ILayerLink on [ParameterMetadata] ([LayerLink])

 ALTER TABLE [MapObjectSetting] ADD CONSTRAINT [MapObjectSetting_FMap_0] FOREIGN KEY ([DefaultMap]) REFERENCES [Map]
CREATE INDEX MapObjectSetting_IDefaultMap on [MapObjectSetting] ([DefaultMap])

 ALTER TABLE [LinkParameter] ADD CONSTRAINT [LinkParameter_FLayerLink_0] FOREIGN KEY ([LayerLink]) REFERENCES [LayerLink]
CREATE INDEX LinkParameter_ILayerLink on [LinkParameter] ([LayerLink])

 ALTER TABLE [DataLinkParameter] ADD CONSTRAINT [DataLinkParameter_FDataLink_0] FOREIGN KEY ([Link]) REFERENCES [DataLink]
CREATE INDEX DataLinkParameter_ILink on [DataLinkParameter] ([Link])

 ALTER TABLE [LayerLink] ADD CONSTRAINT [LayerLink_FMapObjectSetting_0] FOREIGN KEY ([MapObjectSetting]) REFERENCES [MapObjectSetting]
CREATE INDEX LayerLink_IMapObjectSetting on [LayerLink] ([MapObjectSetting])

 ALTER TABLE [LayerLink] ADD CONSTRAINT [LayerLink_FMapLayer_0] FOREIGN KEY ([Layer]) REFERENCES [MapLayer]
CREATE INDEX LayerLink_ILayer on [LayerLink] ([Layer])

 ALTER TABLE [STORMWEBSEARCH] ADD CONSTRAINT [STORMWEBSEARCH_FSTORMFILTERSETTING_0] FOREIGN KEY ([FilterSetting_m0]) REFERENCES [STORMFILTERSETTING]

 ALTER TABLE [STORMFILTERDETAIL] ADD CONSTRAINT [STORMFILTERDETAIL_FSTORMFILTERSETTING_0] FOREIGN KEY ([FilterSetting_m0]) REFERENCES [STORMFILTERSETTING]

 ALTER TABLE [STORMFILTERLOOKUP] ADD CONSTRAINT [STORMFILTERLOOKUP_FSTORMFILTERSETTING_0] FOREIGN KEY ([FilterSetting_m0]) REFERENCES [STORMFILTERSETTING]

/****** Pre-script END ******/





CREATE TABLE [Address] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Place] VARCHAR(255)  NULL,

	 [Location] GEOMETRY  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Request] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Date] DATETIME  NULL,

	 [Urgently] BIT  NULL,

	 [Task] VARCHAR(255)  NULL,

	 [Author] UNIQUEIDENTIFIER  NOT NULL,

	 [Address] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Comment] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Text] VARCHAR(255)  NULL,

	 [Type] VARCHAR(12)  NULL,

	 [Request] UNIQUEIDENTIFIER  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [Author] (

	 [primaryKey] UNIQUEIDENTIFIER  NOT NULL,

	 [Name] VARCHAR(255)  NULL,

	 [Phone] INT  NULL,

	 [Email] VARCHAR(255)  NULL,

	 [Birthday] DATETIME  NULL,

	 [Gender] VARCHAR(7)  NULL,

	 [Vip] BIT  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMNETLOCKDATA] (

	 [LockKey] VARCHAR(300)  NOT NULL,

	 [UserName] VARCHAR(300)  NOT NULL,

	 [LockDate] DATETIME  NULL,

	 PRIMARY KEY ([LockKey]))


CREATE TABLE [STORMSETTINGS] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Module] varchar(1000)  NULL,

	 [Name] varchar(255)  NULL,

	 [Value] text  NULL,

	 [User] varchar(255)  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMAdvLimit] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [User] varchar(255)  NULL,

	 [Published] bit  NULL,

	 [Module] varchar(255)  NULL,

	 [Name] varchar(255)  NULL,

	 [Value] text  NULL,

	 [HotKeyData] int  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMFILTERSETTING] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Name] varchar(255)  NOT NULL,

	 [DataObjectView] varchar(255)  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMWEBSEARCH] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Name] varchar(255)  NOT NULL,

	 [Order] INT  NOT NULL,

	 [PresentView] varchar(255)  NOT NULL,

	 [DetailedView] varchar(255)  NOT NULL,

	 [FilterSetting_m0] uniqueidentifier  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMFILTERDETAIL] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Caption] varchar(255)  NOT NULL,

	 [DataObjectView] varchar(255)  NOT NULL,

	 [ConnectMasterProp] varchar(255)  NOT NULL,

	 [OwnerConnectProp] varchar(255)  NULL,

	 [FilterSetting_m0] uniqueidentifier  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [STORMFILTERLOOKUP] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [DataObjectType] varchar(255)  NOT NULL,

	 [Container] varchar(255)  NULL,

	 [ContainerTag] varchar(255)  NULL,

	 [FieldsToView] varchar(255)  NULL,

	 [FilterSetting_m0] uniqueidentifier  NOT NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [UserSetting] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [AppName] varchar(256)  NULL,

	 [UserName] varchar(512)  NULL,

	 [UserGuid] uniqueidentifier  NULL,

	 [ModuleName] varchar(1024)  NULL,

	 [ModuleGuid] uniqueidentifier  NULL,

	 [SettName] varchar(256)  NULL,

	 [SettGuid] uniqueidentifier  NULL,

	 [SettLastAccessTime] DATETIME  NULL,

	 [StrVal] varchar(256)  NULL,

	 [TxtVal] varchar(max)  NULL,

	 [IntVal] int  NULL,

	 [BoolVal] bit  NULL,

	 [GuidVal] uniqueidentifier  NULL,

	 [DecimalVal] decimal(20,10)  NULL,

	 [DateTimeVal] DATETIME  NULL,

	 PRIMARY KEY ([primaryKey]))


CREATE TABLE [ApplicationLog] (

	 [primaryKey] uniqueidentifier  NOT NULL,

	 [Category] varchar(64)  NULL,

	 [EventId] INT  NULL,

	 [Priority] INT  NULL,

	 [Severity] varchar(32)  NULL,

	 [Title] varchar(256)  NULL,

	 [Timestamp] DATETIME  NULL,

	 [MachineName] varchar(32)  NULL,

	 [AppDomainName] varchar(512)  NULL,

	 [ProcessId] varchar(256)  NULL,

	 [ProcessName] varchar(512)  NULL,

	 [ThreadName] varchar(512)  NULL,

	 [Win32ThreadId] varchar(128)  NULL,

	 [Message] varchar(2500)  NULL,

	 [FormattedMessage] varchar(max)  NULL,

	 PRIMARY KEY ([primaryKey]))




 ALTER TABLE [Request] ADD CONSTRAINT [Request_FAuthor_0] FOREIGN KEY ([Author]) REFERENCES [Author]
CREATE INDEX Request_IAuthor on [Request] ([Author])

 ALTER TABLE [Request] ADD CONSTRAINT [Request_FAddress_0] FOREIGN KEY ([Address]) REFERENCES [Address]
CREATE INDEX Request_IAddress on [Request] ([Address])

 ALTER TABLE [Comment] ADD CONSTRAINT [Comment_FRequest_0] FOREIGN KEY ([Request]) REFERENCES [Request]
CREATE INDEX Comment_IRequest on [Comment] ([Request])

 ALTER TABLE [STORMWEBSEARCH] ADD CONSTRAINT [STORMWEBSEARCH_FSTORMFILTERSETTING_0] FOREIGN KEY ([FilterSetting_m0]) REFERENCES [STORMFILTERSETTING]

 ALTER TABLE [STORMFILTERDETAIL] ADD CONSTRAINT [STORMFILTERDETAIL_FSTORMFILTERSETTING_0] FOREIGN KEY ([FilterSetting_m0]) REFERENCES [STORMFILTERSETTING]

 ALTER TABLE [STORMFILTERLOOKUP] ADD CONSTRAINT [STORMFILTERLOOKUP_FSTORMFILTERSETTING_0] FOREIGN KEY ([FilterSetting_m0]) REFERENCES [STORMFILTERSETTING]

