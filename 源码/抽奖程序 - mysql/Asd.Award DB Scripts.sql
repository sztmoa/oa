

/*==============================================================*/
/* Table: TmpAward                                              */
/*==============================================================*/
create table TmpAward
(
   Level                national varchar(50),
   TicketNO             national varchar(50) not null,
   Remark               national varchar(50),
   UpdateTime           datetime,
   primary key (TicketNO)
);

/*==============================================================*/
/* Table: TmpOption                                             */
/*==============================================================*/
create table TmpOption
(
   BelongTo             varchar(2) not null,
   Level                char(1) not null,
   HitSequence          int not null,
   AwardQty             int not null,
   Remark               national varchar(50),
   primary key (BelongTo, Level, HitSequence)
);

/*==============================================================*/
/* Table: TmpSetting                                            */
/*==============================================================*/
create table TmpSetting
(
   SettingName          national varchar(50) not null,
   SettingValue         national varchar(50),
   Remark               national varchar(50),
   primary key (SettingName)
);

/*==============================================================*/
/* Table: TmpTicket                                             */
/*==============================================================*/
create table TmpTicket
(
   TicketNO             national varchar(50) not null,
   TicketCount          national varchar(50),
   primary key (TicketNO)
);
