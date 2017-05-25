CREATE TABLE [dbo].[SQLManagerPaidHistory]
(
[F201] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F402] [float] NULL,
[F401] [datetime] NULL,
[F101] [float] NULL,
[F102] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F103] [float] NULL,
[F104] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F301] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F302] [nvarchar] (45) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F303] [nvarchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F304] [datetime] NULL,
[F305] [float] NULL,
[F306] [float] NULL,
[F308] [float] NULL,
[F307] [float] NULL,
[F309] [float] NULL,
[F310] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F311] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F403] [float] NULL,
[F404] [float] NULL,
[F405] [float] NULL,
[F406] [float] NULL,
[F407] [float] NULL,
[F408] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F409] [decimal] (15, 5) NULL,
[F411] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F414] [datetime] NULL,
[F415] [float] NULL,
[F419] [float] NULL,
[F420] [float] NULL,
[F426] [decimal] (15, 5) NULL,
[F410] [decimal] (15, 5) NULL,
[F412] [decimal] (15, 5) NULL,
[F416] [float] NULL,
[F418] [float] NULL,
[F421] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F422] [float] NULL,
[F423] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F424] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F430] [decimal] (15, 5) NULL,
[F433] [decimal] (15, 5) NULL,
[F439] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F440] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F441] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F442] [float] NULL,
[F524] [nvarchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F505] [decimal] (15, 5) NULL,
[F506] [decimal] (15, 5) NULL,
[F507] [decimal] (15, 5) NULL,
[F508] [decimal] (15, 5) NULL,
[F509] [decimal] (15, 5) NULL,
[F503] [nvarchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F504] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F512] [decimal] (15, 5) NULL,
[F513] [decimal] (15, 5) NULL,
[F514] [decimal] (15, 5) NULL,
[F517] [decimal] (15, 5) NULL,
[F518] [decimal] (15, 5) NULL,
[F519] [decimal] (15, 5) NULL,
[F520] [decimal] (15, 5) NULL,
[F521] [decimal] (15, 5) NULL,
[F522] [float] NULL,
[F523] [decimal] (15, 5) NULL,
[F525] [nvarchar] (160) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F526] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CYCLEDATE] [datetime] NULL,
[CYCLETIME] [datetime] NULL,
[TRANSACTIONTYPE] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ENTEREDDATE] [datetime] NULL,
[AGEHANDLER] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AGEHWHY] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[COPAYHANDLER] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[COPAYHWHY] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[COSTHANDLER] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[COSTHWHY] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DISPFEEHANDLER] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DISPFEEHWHY] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DISPLIMHANDLER] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DISPLIMHWHY] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[REFILLHANDLER] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[REFILLHWHY] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PROFFEEHANDLER] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PROFFEEHWHY] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TOTALBENEFIT] [decimal] (15, 5) NULL,
[DAWDIFF] [decimal] (15, 5) NULL,
[DAWWHO] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PC] [float] NULL,
[DISPFEE] [decimal] (15, 5) NULL,
[COSTUSED] [float] NULL,
[COSTDESC] [nvarchar] (6) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PROFFEE] [decimal] (15, 5) NULL,
[PHARMNAME] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CHAINCODE] [nvarchar] (4) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ADMINFEE] [decimal] (15, 5) NULL,
[MONY] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DEA] [nvarchar] (6) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[GPI] [nvarchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LOCATIONCODE] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[GROUPNUM] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CARRIER] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PRESCRIBER] [nvarchar] (9) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DRUGNAME] [nvarchar] (45) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MARKUP] [decimal] (15, 5) NULL,
[MARKUPPERCENT] [float] NULL,
[MAINTFLAG] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DESI] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F106] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F107] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BENEFITKEY] [float] NULL,
[OUTOFPOCKET] [decimal] (15, 5) NULL,
[MISCHANDLER] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MISCHWHY] [nvarchar] (60) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DEDUCTIBLE] [decimal] (15, 5) NULL,
[AUTHNUM] [float] NULL,
[LASTNAME] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FIRSTNAME] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DOB] [datetime] NULL,
[PREFFLAG] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RXEND] [datetime] NULL,
[PLAN] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AWPUNITPRICE] [decimal] (15, 5) NULL,
[HCFAUNITPRICE] [decimal] (15, 5) NULL,
[F434] [datetime] NULL,
[F435] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F315] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F316] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F317] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F318] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F319] [nvarchar] (9) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F320] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F322] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F323] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F324] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F325] [nvarchar] (9) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F326] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F329] [nvarchar] (9) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ADDLCOST] [decimal] (15, 5) NULL,
[UM001] [decimal] (15, 5) NULL,
[UM002] [decimal] (15, 5) NULL,
[UM003] [decimal] (15, 5) NULL,
[UM004] [decimal] (15, 5) NULL,
[UM005] [decimal] (15, 5) NULL,
[UM006] [decimal] (15, 5) NULL,
[UM007] [decimal] (15, 5) NULL,
[UM008] [decimal] (15, 5) NULL,
[UM009] [decimal] (15, 5) NULL,
[UM010] [decimal] (15, 5) NULL,
[UD001] [datetime] NULL,
[UD002] [datetime] NULL,
[UD003] [datetime] NULL,
[UD004] [datetime] NULL,
[UD005] [datetime] NULL,
[UN001] [float] NULL,
[UN002] [float] NULL,
[UN003] [float] NULL,
[UN004] [float] NULL,
[UN005] [float] NULL,
[UT001] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UT002] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UT003] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UT004] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UT005] [nvarchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F109] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F202] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F110] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F331] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F332] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F333] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F334] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F335] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F312] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F313] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F314] [nvarchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F336] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F455] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F436] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F456] [nvarchar] (9) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F457] [nvarchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F458] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F459] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F460] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F429] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F453] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F445] [nvarchar] (19) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F446] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F330] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F454] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F600] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F461] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F462] [nvarchar] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F463] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F464] [nvarchar] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F343] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F344] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F345] [nvarchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F465] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F444] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F466] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F467] [nvarchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F427] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F498] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F468] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F469] [nvarchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F470] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F337] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F338] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F339] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F340] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F443] [nvarchar] (8) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F341] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F342] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F471] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F472] [nvarchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F321] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F327] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F474] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F475] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F476] [nvarchar] (19) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F477] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F438] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F478] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F479] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F480] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F481] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F482] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F483] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F484] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F485] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F486] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F487] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F450] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F451] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F452] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F447] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F488] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F489] [nvarchar] (19) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F448] [nvarchar] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F449] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F490] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[COMPOUNDSEG] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F750] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F751] [nvarchar] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F752] [nvarchar] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F753] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F754] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F755] [nvarchar] (12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F756] [nvarchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F757] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F758] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F759] [nvarchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F760] [nvarchar] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F761] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F491] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F492] [nvarchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F493] [nvarchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
