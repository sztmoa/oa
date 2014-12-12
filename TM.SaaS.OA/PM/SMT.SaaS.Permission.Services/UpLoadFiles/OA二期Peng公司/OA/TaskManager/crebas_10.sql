/*==============================================================*/
/* DBMS name:      ORACLE Version 10g                           */
/* Created on:     2010-8-23 15:49:06                           */
/*==============================================================*/


alter table SMTLM.T_LM_ACCIDENT
   drop constraint FK_T_LM_ACC_REFERENCE_T_LM_WAR;

alter table SMTLM.T_LM_BATCHSET
   drop constraint FK_T_LM_BAT_REFERENCE_T_LM_GOO;

alter table SMTLM.T_LM_INSTOCKATTACH
   drop constraint T_LM_INSTOCKA_FK21271040579953;

alter table SMTLM.T_LM_INSTOCKDETAIL
   drop constraint T_LM_INSTOCKD_FK21264053987906;

alter table SMTLM.T_LM_INSTOCKDETAIL
   drop constraint T_LM_INSTOCKD_FK41264055866125;

alter table SMTLM.T_LM_ORDERDETAIL
   drop constraint FK_T_LM_ORD_REFERENCE_T_LM_ORD;

alter table SMTLM.T_LM_OUTSTOCKDETAIL
   drop constraint T_LM_OUTSTOCK_FK21264054157968;

alter table SMTLM.T_LM_OUTSTOCKDETAIL
   drop constraint T_LM_OUTSTOCK_FK31271225352859;

alter table SMTLM.T_LM_OUTSTOCKMASTER
   drop constraint T_LM_OUTSTOCK_FK31264123262296;

alter table SMTLM.T_LM_OUTSTOCKPACKINGLISTDETAIL
   drop constraint FK_T_LM_OUT_REFERENCE_T_LM_GOO;

alter table SMTLM.T_LM_OUTSTOCKPACKINGLISTDETAIL
   drop constraint T_LM_OUTSTOCK_FK31271039970828;

alter table SMTLM.T_LM_OUTSTOCKPACKINGLISTMASTER
   drop constraint T_LM_OUTSTOCK_FK21271039844359;

alter table SMTLM.T_LM_PACKAGE
   drop constraint MMM;

alter table SMTLM.T_LM_PICKDETAIL
   drop constraint FK_T_LM_PIC_REFERENCE_T_LM_GOO;

alter table SMTLM.T_LM_PICKDETAIL
   drop constraint FK_T_LM_PIC_REFERENCE_T_LM_OUT;

alter table SMTLM.T_LM_PICKDETAIL
   drop constraint FK_T_LM_PIC_REFERENCE_T_LM_PIC;

alter table SMTLM.T_LM_PICKGOODSPACKAGE
   drop constraint AAA;

alter table SMTLM.T_LM_PICKMASTER
   drop constraint T_LM_OUTSTOCKM_FK_PICKMASTER;

alter table SMTLM.T_LM_PUTUP
   drop constraint T_LM_PUTUP_FK21271041275406;

alter table SMTLM.T_LM_PUTUP
   drop constraint T_LM_PUTUP_FK31271041299750;

alter table SMTLM.T_LM_STOCKCOUNTPLANDETAIL
   drop constraint FK_T_LM_STO_REFERENCE_T_LM_STO;

alter table SMTLM.T_LM_WAREHOUSELOCATION
   drop constraint T_LM_WAREHOUS_FK31271040473890;

alter table SMTLM.T_LM_ORDERDETAIL
   drop primary key cascade;

drop table SMTLM."tmp_T_LM_ORDERDETAIL" cascade constraints;

rename T_LM_ORDERDETAIL to "tmp_T_LM_ORDERDETAIL";

alter table SMTLM.T_LM_PREFIX
   drop primary key cascade;

drop user SMTLM;

/*==============================================================*/
/* Table: T_LM_ALERTMESSAGE                                     */
/*==============================================================*/
create table T_LM_ALERTMESSAGE  (
   ALERTID              NVARCHAR2(50)                   not null,
   CONSIGNERID          NVARCHAR2(50)                   not null,
   CONTENT              NVARCHAR2(2000),
   ALERTTIME            DATE,
   ALERTOBJ             NVARCHAR2(50)                   not null,
   OBJECTTYPE           NUMBER(1)                       not null,
   MESSAGETYPE          NUMBER(1)                       not null,
   ALERTSTATE           NUMBER(1)                      default 0 not null,
   SYSTEM               NUMBER(1)                      default 0 not null,
   ISMESSAGE            NUMBER(1)                      default 0 not null,
   ISIM                 NUMBER(1)                      default 0 not null,
   EMAIL                NUMBER(1)                      default 0 not null,
   CHECKSTATE           NVARCHAR2(50),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_ALERTMESSAGE primary key (ALERTID)
);

comment on table T_LM_ALERTMESSAGE is
'1-21自动跟踪设置表';

comment on column T_LM_ALERTMESSAGE.ALERTID is
'自动跟踪设置ID';

comment on column T_LM_ALERTMESSAGE.CONSIGNERID is
'委托客户';

comment on column T_LM_ALERTMESSAGE.CONTENT is
'发送内容格式';

comment on column T_LM_ALERTMESSAGE.ALERTTIME is
'提醒时间';

comment on column T_LM_ALERTMESSAGE.ALERTOBJ is
'提醒对象';

comment on column T_LM_ALERTMESSAGE.OBJECTTYPE is
'对象类型';

comment on column T_LM_ALERTMESSAGE.MESSAGETYPE is
'消息类型';

comment on column T_LM_ALERTMESSAGE.ALERTSTATE is
'提醒状态';

comment on column T_LM_ALERTMESSAGE.SYSTEM is
'系统提醒';

comment on column T_LM_ALERTMESSAGE.ISMESSAGE is
'短信提醒';

comment on column T_LM_ALERTMESSAGE.ISIM is
'即时消息';

comment on column T_LM_ALERTMESSAGE.EMAIL is
'邮件提醒';

comment on column T_LM_ALERTMESSAGE.CHECKSTATE is
'审核状态';

comment on column T_LM_ALERTMESSAGE.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_ALERTMESSAGE.CREATEDATE is
'CREATEDATE';

comment on column T_LM_ALERTMESSAGE.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_ALERTMESSAGE.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_ALERTMESSAGE.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_ALERTMESSAGE.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_ALERTMESSAGE.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_ALERTMESSAGE.OWNERID is
'OWNERID';

comment on column T_LM_ALERTMESSAGE.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_ALERTMESSAGE.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_ALERTMESSAGE.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_ALERTMESSAGELOG                                  */
/*==============================================================*/
create table T_LM_ALERTMESSAGELOG  (
   ALERTLOGID           NVARCHAR2(50)                   not null,
   CONTENT              NVARCHAR2(2000),
   INTIME               DATE                           default SYSDATE not null,
   ALERTOBJ             NVARCHAR2(50)                   not null,
   ALERTSTATE           NUMBER(1)                      default 0 not null,
   ALERTMODE            NUMBER(1)                      default 0 not null,
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_ALERTMESSAGELOG primary key (ALERTLOGID)
);

comment on table T_LM_ALERTMESSAGELOG is
'1-20自动跟踪记录表';

comment on column T_LM_ALERTMESSAGELOG.ALERTLOGID is
'自动跟踪记录ID';

comment on column T_LM_ALERTMESSAGELOG.CONTENT is
'发送内容';

comment on column T_LM_ALERTMESSAGELOG.INTIME is
'发送时间';

comment on column T_LM_ALERTMESSAGELOG.ALERTOBJ is
'发送对象';

comment on column T_LM_ALERTMESSAGELOG.ALERTSTATE is
'提醒状态';

comment on column T_LM_ALERTMESSAGELOG.ALERTMODE is
'提醒方式';

comment on column T_LM_ALERTMESSAGELOG.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_ALERTMESSAGELOG.CREATEDATE is
'CREATEDATE';

comment on column T_LM_ALERTMESSAGELOG.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_ALERTMESSAGELOG.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_ALERTMESSAGELOG.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_ALERTMESSAGELOG.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_ALERTMESSAGELOG.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_ALERTMESSAGELOG.OWNERID is
'OWNERID';

comment on column T_LM_ALERTMESSAGELOG.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_ALERTMESSAGELOG.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_ALERTMESSAGELOG.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_APPEAL                                           */
/*==============================================================*/
create table T_LM_APPEAL  (
   APPEALID             NVARCHAR2(50)                   not null,
   APPEALCODE           NVARCHAR2(50)                   not null,
   ORDERMASTERID        NVARCHAR2(50),
   CUSTOMERID           NVARCHAR2(50),
   CUSTOMERNAME         NVARCHAR2(100)                  not null,
   TITLE                NVARCHAR2(50),
   APPEALTYPE           NVARCHAR2(20),
   APPEALSTEP           NVARCHAR2(20),
   BEWRITE              NVARCHAR2(2000),
   DISPOSALDISCRIPTION  NVARCHAR2(2000),
   DISPOSALSTATE        NUMBER                         default 1,
   DISPOSALTIME         NUMBER,
   RESPONSETIME         NUMBER,
   DISPOSALRESULT       NVARCHAR2(50),
   CHECKSTATE           NVARCHAR2(50),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_APPEAL primary key (APPEALID, CUSTOMERNAME)
);

comment on table T_LM_APPEAL is
'1-17客户投诉/建议表';

comment on column T_LM_APPEAL.APPEALID is
'投诉/建议单号ID';

comment on column T_LM_APPEAL.APPEALCODE is
'投诉/建议单号';

comment on column T_LM_APPEAL.ORDERMASTERID is
'订单号';

comment on column T_LM_APPEAL.CUSTOMERID is
'客户代码';

comment on column T_LM_APPEAL.CUSTOMERNAME is
'客户名称';

comment on column T_LM_APPEAL.TITLE is
'投诉/建议标题';

comment on column T_LM_APPEAL.APPEALTYPE is
'投诉/建议类型';

comment on column T_LM_APPEAL.APPEALSTEP is
'投诉/建议等级';

comment on column T_LM_APPEAL.BEWRITE is
'详细描述';

comment on column T_LM_APPEAL.DISPOSALDISCRIPTION is
'处理描述';

comment on column T_LM_APPEAL.DISPOSALSTATE is
'处理状态';

comment on column T_LM_APPEAL.DISPOSALTIME is
'处理时效';

comment on column T_LM_APPEAL.RESPONSETIME is
'响应时效';

comment on column T_LM_APPEAL.DISPOSALRESULT is
'处理结果';

comment on column T_LM_APPEAL.CHECKSTATE is
'审核状态';

comment on column T_LM_APPEAL.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_APPEAL.CREATEDATE is
'CREATEDATE';

comment on column T_LM_APPEAL.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_APPEAL.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_APPEAL.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_APPEAL.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_APPEAL.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_APPEAL.OWNERID is
'OWNERID';

comment on column T_LM_APPEAL.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_APPEAL.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_APPEAL.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_ATTACH                                           */
/*==============================================================*/
create table T_LM_ATTACH  (
   ALERTID              NVARCHAR2(50)                   not null,
   ATTACHID             NVARCHAR2(50)                   not null,
   ATTACHNAME           NVARCHAR2(50)                   not null,
   ATTACHBODY           blob,
   ATTACHSOURCE         NVARCHAR2(50),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_ATTACH primary key (ALERTID)
);

comment on table T_LM_ATTACH is
'1-18公共附件表';

comment on column T_LM_ATTACH.ALERTID is
'主键ID';

comment on column T_LM_ATTACH.ATTACHID is
'附件ID';

comment on column T_LM_ATTACH.ATTACHNAME is
'附件名称';

comment on column T_LM_ATTACH.ATTACHBODY is
'附件主体';

comment on column T_LM_ATTACH.ATTACHSOURCE is
'附件来源';

comment on column T_LM_ATTACH.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_ATTACH.CREATEDATE is
'CREATEDATE';

comment on column T_LM_ATTACH.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_ATTACH.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_ATTACH.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_ATTACH.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_ATTACH.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_ATTACH.OWNERID is
'OWNERID';

comment on column T_LM_ATTACH.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_ATTACH.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_ATTACH.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_CARRYSIGN                                        */
/*==============================================================*/
create table T_LM_CARRYSIGN  (
   CARRYSIGNID          NVARCHAR2(50)                   not null,
   ORDERMASTERID        NVARCHAR2(50)                   not null,
   CARRYID              NVARCHAR2(50)                   not null,
   ISPROXY              NUMBER                         default 0 not null,
   SIGNER               NVARCHAR2(50),
   SIGNTIME             DATE                           default SYSDATE,
   SIGNDATE             NVARCHAR2(50),
   DELIVERCODE          NVARCHAR2(50),
   DELIVER              NVARCHAR2(50),
   REQUEST              NVARCHAR2(50)                   not null,
   CARRYSIGNSTATE       NUMBER(50)                      not null,
   ISINSTEADGET         NUMBER                         default 0 not null,
   NODECODE             NVARCHAR2(50),
   REMARK               NVARCHAR2(2000),
   SWATCHATTACHID       NVARCHAR2(50)                   not null,
   ATTACHID             NVARCHAR2(50)                   not null,
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_CARRYSIGN primary key (CARRYSIGNID)
);

comment on table T_LM_CARRYSIGN is
'1-8签单回执表';

comment on column T_LM_CARRYSIGN.CARRYSIGNID is
'回执ID';

comment on column T_LM_CARRYSIGN.ORDERMASTERID is
'订单ID';

comment on column T_LM_CARRYSIGN.CARRYID is
'运单号';

comment on column T_LM_CARRYSIGN.ISPROXY is
'是否代理签收';

comment on column T_LM_CARRYSIGN.SIGNER is
'签收人姓名';

comment on column T_LM_CARRYSIGN.SIGNTIME is
'签收时间';

comment on column T_LM_CARRYSIGN.SIGNDATE is
'签收日期';

comment on column T_LM_CARRYSIGN.DELIVERCODE is
'派件人代码';

comment on column T_LM_CARRYSIGN.DELIVER is
'派件人';

comment on column T_LM_CARRYSIGN.REQUEST is
'回执要求';

comment on column T_LM_CARRYSIGN.CARRYSIGNSTATE is
'回执状态';

comment on column T_LM_CARRYSIGN.ISINSTEADGET is
'代收货款';

comment on column T_LM_CARRYSIGN.NODECODE is
'网点代码';

comment on column T_LM_CARRYSIGN.REMARK is
'备注';

comment on column T_LM_CARRYSIGN.SWATCHATTACHID is
'样本附件ID';

comment on column T_LM_CARRYSIGN.ATTACHID is
'附件ID';

comment on column T_LM_CARRYSIGN.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_CARRYSIGN.CREATEDATE is
'CREATEDATE';

comment on column T_LM_CARRYSIGN.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_CARRYSIGN.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_CARRYSIGN.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_CARRYSIGN.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_CARRYSIGN.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_CARRYSIGN.OWNERID is
'OWNERID';

comment on column T_LM_CARRYSIGN.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_CARRYSIGN.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_CARRYSIGN.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_CUSTOMER                                         */
/*==============================================================*/
create table T_LM_CUSTOMER  (
   CUSTNAME             NVARCHAR2(50)                   not null,
   CUSTCODE             NVARCHAR2(50)                   not null,
   SHORTPINYIN          NVARCHAR2(50),
   ENNAME               NVARCHAR2(50)                   not null,
   ADDRESS              NVARCHAR2(200)                  not null,
   TEL                  NVARCHAR2(50)                   not null,
   FAX                  NVARCHAR2(50),
   EMAIL                NVARCHAR2(100),
   POSTCODE             NVARCHAR2(50),
   HOMEPAGE             NVARCHAR2(100),
   CORPTYPE             NVARCHAR2(50),
   CORPTYPENAME         NVARCHAR2(100),
   BUSINESSTYPE         NUMBER(1)                      default 1 not null,
   BUSINESSTYPENAME     NVARCHAR2(100),
   PRODUCTKIND          NVARCHAR2(50)                   not null,
   PRODUCTKINDNAME      NVARCHAR2(100),
   REGISTERFUND         NVARCHAR2(50),
   COOPERATESTARTTIME   DATE,
   COOPERATEENDTIME     DATE,
   COOPERATESTATE       NUMBER                          not null,
   SHOTNAME             NVARCHAR2(50)                   not null,
   BELONGORGCODE        NUMBER(3)                      default 0 not null,
   SHOPCARD             NVARCHAR2(50),
   CORPORATION          NVARCHAR2(50),
   ORGCODE              NVARCHAR2(50),
   REMARK               NVARCHAR2(2000),
   ATTACHID             NVARCHAR2(50)                   not null,
   CHECKSTATE           NVARCHAR2(50),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_CUSTOMER primary key (CUSTCODE)
);

comment on table T_LM_CUSTOMER is
'1-1客户档案维护';

comment on column T_LM_CUSTOMER.CUSTNAME is
'客户名称';

comment on column T_LM_CUSTOMER.CUSTCODE is
'客户代码';

comment on column T_LM_CUSTOMER.SHORTPINYIN is
'拼音简称';

comment on column T_LM_CUSTOMER.ENNAME is
'英文简称';

comment on column T_LM_CUSTOMER.ADDRESS is
'地址';

comment on column T_LM_CUSTOMER.TEL is
'电话';

comment on column T_LM_CUSTOMER.FAX is
'传真';

comment on column T_LM_CUSTOMER.EMAIL is
'EMAIL地址';

comment on column T_LM_CUSTOMER.POSTCODE is
'邮政编码';

comment on column T_LM_CUSTOMER.HOMEPAGE is
'客户主页';

comment on column T_LM_CUSTOMER.CORPTYPE is
'企业类型';

comment on column T_LM_CUSTOMER.CORPTYPENAME is
'企业类型名称';

comment on column T_LM_CUSTOMER.BUSINESSTYPE is
'客户类型';

comment on column T_LM_CUSTOMER.BUSINESSTYPENAME is
'客户类型名称';

comment on column T_LM_CUSTOMER.PRODUCTKIND is
'产品性质';

comment on column T_LM_CUSTOMER.PRODUCTKINDNAME is
'产品性质名称';

comment on column T_LM_CUSTOMER.REGISTERFUND is
'注册资金';

comment on column T_LM_CUSTOMER.COOPERATESTARTTIME is
'开始合作时间';

comment on column T_LM_CUSTOMER.COOPERATEENDTIME is
'合作结束时间';

comment on column T_LM_CUSTOMER.COOPERATESTATE is
'合作状态';

comment on column T_LM_CUSTOMER.SHOTNAME is
'客户简称';

comment on column T_LM_CUSTOMER.BELONGORGCODE is
'客户所属客服';

comment on column T_LM_CUSTOMER.SHOPCARD is
'营业执照号';

comment on column T_LM_CUSTOMER.CORPORATION is
'法人';

comment on column T_LM_CUSTOMER.ORGCODE is
'机构代码';

comment on column T_LM_CUSTOMER.REMARK is
'备注';

comment on column T_LM_CUSTOMER.ATTACHID is
'附件ID';

comment on column T_LM_CUSTOMER.CHECKSTATE is
'审核状态';

comment on column T_LM_CUSTOMER.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_CUSTOMER.CREATEDATE is
'CREATEDATE';

comment on column T_LM_CUSTOMER.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_CUSTOMER.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_CUSTOMER.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_CUSTOMER.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_CUSTOMER.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_CUSTOMER.OWNERID is
'OWNERID';

comment on column T_LM_CUSTOMER.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_CUSTOMER.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_CUSTOMER.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_CUSTOMERADDRESS                                  */
/*==============================================================*/
create table T_LM_CUSTOMERADDRESS  (
   ADDRESSID            VARCHAR2(50)                    not null,
   CUSTOMERCODE         NVARCHAR2(50)                   not null,
   CUSTOMERNAME         NVARCHAR2(50),
   ORGCODE              NVARCHAR2(50)                   not null,
   ORGNAME              NVARCHAR2(50),
   AOBJECT              NVARCHAR2(50),
   AREA                 NVARCHAR2(50)                   not null,
   RECEIVERID           NVARCHAR2(50),
   LINKMAN              NVARCHAR2(50)                   not null,
   TEL                  NVARCHAR2(50)                   not null,
   ADDRESS              NVARCHAR2(2000)                 not null,
   REMARK               NVARCHAR2(50),
   STATE                NUMBER                         default 0,
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_CUSTOMERADDRESS primary key (ADDRESSID)
);

comment on table T_LM_CUSTOMERADDRESS is
'1-19收货方地址维护';

comment on column T_LM_CUSTOMERADDRESS.ADDRESSID is
'地址常用ID';

comment on column T_LM_CUSTOMERADDRESS.CUSTOMERCODE is
'客户代码';

comment on column T_LM_CUSTOMERADDRESS.CUSTOMERNAME is
'客户名称';

comment on column T_LM_CUSTOMERADDRESS.ORGCODE is
'机构代码';

comment on column T_LM_CUSTOMERADDRESS.ORGNAME is
'机构名称';

comment on column T_LM_CUSTOMERADDRESS.AOBJECT is
'运输方式';

comment on column T_LM_CUSTOMERADDRESS.AREA is
'目的区域';

comment on column T_LM_CUSTOMERADDRESS.RECEIVERID is
'收货客户';

comment on column T_LM_CUSTOMERADDRESS.LINKMAN is
'联系人';

comment on column T_LM_CUSTOMERADDRESS.TEL is
'电话';

comment on column T_LM_CUSTOMERADDRESS.ADDRESS is
'目的地址';

comment on column T_LM_CUSTOMERADDRESS.REMARK is
'备注';

comment on column T_LM_CUSTOMERADDRESS.STATE is
'状态';

comment on column T_LM_CUSTOMERADDRESS.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_CUSTOMERADDRESS.CREATEDATE is
'CREATEDATE';

comment on column T_LM_CUSTOMERADDRESS.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_CUSTOMERADDRESS.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_CUSTOMERADDRESS.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_CUSTOMERADDRESS.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_CUSTOMERADDRESS.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_CUSTOMERADDRESS.OWNERID is
'OWNERID';

comment on column T_LM_CUSTOMERADDRESS.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_CUSTOMERADDRESS.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_CUSTOMERADDRESS.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_CUSTOMERBANK                                     */
/*==============================================================*/
create table T_LM_CUSTOMERBANK  (
   ACCOUNTSID           NVARCHAR2(50)                   not null,
   CUSTNAME             NVARCHAR2(100),
   BANK                 NVARCHAR2(50),
   BANKADDRESS          NVARCHAR2(50)                  default '1',
   HOLDER               NVARCHAR2(50),
   IDENTITYCARD         NVARCHAR2(50),
   BANKACCOUNTS         NVARCHAR2(50),
   REMARK               NVARCHAR2(100),
   USESTATE             NVARCHAR2(50),
   CHECKSTATE           NVARCHAR2(50),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_CUSTOMERBANK primary key (ACCOUNTSID)
);

comment on table T_LM_CUSTOMERBANK is
'1-3客户银行帐户表';

comment on column T_LM_CUSTOMERBANK.ACCOUNTSID is
'客户银行帐号ID';

comment on column T_LM_CUSTOMERBANK.CUSTNAME is
'客户名称';

comment on column T_LM_CUSTOMERBANK.BANK is
'开户银行';

comment on column T_LM_CUSTOMERBANK.BANKADDRESS is
'开户银行地址';

comment on column T_LM_CUSTOMERBANK.HOLDER is
'开户人';

comment on column T_LM_CUSTOMERBANK.IDENTITYCARD is
'开户人身份证号';

comment on column T_LM_CUSTOMERBANK.BANKACCOUNTS is
'银行帐号';

comment on column T_LM_CUSTOMERBANK.REMARK is
'备注';

comment on column T_LM_CUSTOMERBANK.USESTATE is
'使用状态';

comment on column T_LM_CUSTOMERBANK.CHECKSTATE is
'审核状态';

comment on column T_LM_CUSTOMERBANK.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_CUSTOMERBANK.CREATEDATE is
'CREATEDATE';

comment on column T_LM_CUSTOMERBANK.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_CUSTOMERBANK.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_CUSTOMERBANK.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_CUSTOMERBANK.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_CUSTOMERBANK.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_CUSTOMERBANK.OWNERID is
'OWNERID';

comment on column T_LM_CUSTOMERBANK.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_CUSTOMERBANK.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_CUSTOMERBANK.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_CUSTOMERLINKMAN                                  */
/*==============================================================*/
create table T_LM_CUSTOMERLINKMAN  (
   LINKMANID            NVARCHAR2(50)                   not null,
   CUSTNAME             NVARCHAR2(50),
   NAME                 NVARCHAR2(50),
   JOB                  NVARCHAR2(50),
   TELEPHONE            NVARCHAR2(50),
   MOBILEPHONE          NVARCHAR2(50),
   ADDRESS              NVARCHAR2(50),
   MAIL                 NVARCHAR2(50),
   开发客户人员ID             NVARCHAR2(50),
   开发客户人员名称             NVARCHAR2(50),
   USESTATE             NVARCHAR2(50)                  default '1',
   REMARK               NVARCHAR2(50),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_CUSTOMERLINKMAN primary key (LINKMANID)
);

comment on table T_LM_CUSTOMERLINKMAN is
'1-2客户联系人表';

comment on column T_LM_CUSTOMERLINKMAN.LINKMANID is
'客户联系人表ID';

comment on column T_LM_CUSTOMERLINKMAN.CUSTNAME is
'客户名称';

comment on column T_LM_CUSTOMERLINKMAN.NAME is
'联系人';

comment on column T_LM_CUSTOMERLINKMAN.JOB is
'职位';

comment on column T_LM_CUSTOMERLINKMAN.TELEPHONE is
'联系电话';

comment on column T_LM_CUSTOMERLINKMAN.MOBILEPHONE is
'手机';

comment on column T_LM_CUSTOMERLINKMAN.ADDRESS is
'联系地址';

comment on column T_LM_CUSTOMERLINKMAN.MAIL is
'邮件地址';

comment on column T_LM_CUSTOMERLINKMAN.开发客户人员ID is
'开发客户人员ID';

comment on column T_LM_CUSTOMERLINKMAN.开发客户人员名称 is
'开发客户人员名称';

comment on column T_LM_CUSTOMERLINKMAN.USESTATE is
'用户状态';

comment on column T_LM_CUSTOMERLINKMAN.REMARK is
'备注';

comment on column T_LM_CUSTOMERLINKMAN.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_CUSTOMERLINKMAN.CREATEDATE is
'CREATEDATE';

comment on column T_LM_CUSTOMERLINKMAN.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_CUSTOMERLINKMAN.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_CUSTOMERLINKMAN.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_CUSTOMERLINKMAN.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_CUSTOMERLINKMAN.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_CUSTOMERLINKMAN.OWNERID is
'OWNERID';

comment on column T_LM_CUSTOMERLINKMAN.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_CUSTOMERLINKMAN.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_CUSTOMERLINKMAN.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_CUSTOMERPROJECT                                  */
/*==============================================================*/
create table T_LM_CUSTOMERPROJECT  (
   PROJECTID            NVARCHAR2(50)                   not null,
   CUSTOMERCODE         NVARCHAR2(50)                   not null,
   CUSTOMERNAME         NVARCHAR2(50),
   PROJECTCODE          NVARCHAR2(50)                   not null,
   PROJECTNAME          NVARCHAR2(50)                   not null,
   SHORT                NVARCHAR2(50),
   STEP                 NVARCHAR2(50),
   REMARK               NVARCHAR2(50),
   STATE                NUMBER(1)                      default 0,
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_CUSTOMERPROJECT primary key (PROJECTID)
);

comment on table T_LM_CUSTOMERPROJECT is
'1-12客户报价方案表';

comment on column T_LM_CUSTOMERPROJECT.PROJECTID is
'方案ID';

comment on column T_LM_CUSTOMERPROJECT.CUSTOMERCODE is
'客户代码';

comment on column T_LM_CUSTOMERPROJECT.CUSTOMERNAME is
'客户名称';

comment on column T_LM_CUSTOMERPROJECT.PROJECTCODE is
'方案代码';

comment on column T_LM_CUSTOMERPROJECT.PROJECTNAME is
'方案名称';

comment on column T_LM_CUSTOMERPROJECT.SHORT is
'字母简写';

comment on column T_LM_CUSTOMERPROJECT.STEP is
'优先等级';

comment on column T_LM_CUSTOMERPROJECT.REMARK is
'备注';

comment on column T_LM_CUSTOMERPROJECT.STATE is
'状态';

comment on column T_LM_CUSTOMERPROJECT.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_CUSTOMERPROJECT.CREATEDATE is
'CREATEDATE';

comment on column T_LM_CUSTOMERPROJECT.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_CUSTOMERPROJECT.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_CUSTOMERPROJECT.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_CUSTOMERPROJECT.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_CUSTOMERPROJECT.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_CUSTOMERPROJECT.OWNERID is
'OWNERID';

comment on column T_LM_CUSTOMERPROJECT.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_CUSTOMERPROJECT.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_CUSTOMERPROJECT.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_CUSTOMERUSER                                     */
/*==============================================================*/
create table T_LM_CUSTOMERUSER  (
   ACCOUNTSID           NVARCHAR2(50)                   not null,
   ACCOUNTS             NVARCHAR2(50),
   NAME                 NVARCHAR2(50),
   PASSWORD             NVARCHAR2(50),
   STATE                NVARCHAR2(50)                  default '1',
   REMARK               NVARCHAR2(2000),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_CUSTOMERUSER primary key (ACCOUNTSID)
);

comment on table T_LM_CUSTOMERUSER is
'1-4客户帐号表';

comment on column T_LM_CUSTOMERUSER.ACCOUNTSID is
'客户帐号ID';

comment on column T_LM_CUSTOMERUSER.ACCOUNTS is
'用户帐号';

comment on column T_LM_CUSTOMERUSER.NAME is
'用户姓名';

comment on column T_LM_CUSTOMERUSER.PASSWORD is
'用户密码';

comment on column T_LM_CUSTOMERUSER.STATE is
'用户状态';

comment on column T_LM_CUSTOMERUSER.REMARK is
'备注';

comment on column T_LM_CUSTOMERUSER.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_CUSTOMERUSER.CREATEDATE is
'CREATEDATE';

comment on column T_LM_CUSTOMERUSER.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_CUSTOMERUSER.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_CUSTOMERUSER.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_CUSTOMERUSER.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_CUSTOMERUSER.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_CUSTOMERUSER.OWNERID is
'OWNERID';

comment on column T_LM_CUSTOMERUSER.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_CUSTOMERUSER.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_CUSTOMERUSER.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_DISTRICTDETAIL                                   */
/*==============================================================*/
create table T_LM_DISTRICTDETAIL  (
   DETAILID             NVARCHAR2(50)                   not null,
   ADDRESSCODE          NVARCHAR2(50)                   not null,
   ADDRESSNAME          NVARCHAR2(50)                   not null,
   PARENTLDID           NVARCHAR2(50),
   BESTROWAREA          NVARCHAR2(50),
   DEFINEAREAID         NVARCHAR2(50),
   DEFINEAREA           NVARCHAR2(50),
   NODEBOUND            NVARCHAR2(50),
   AVAILLIMIT           NVARCHAR2(50),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_DISTRICTDETAIL primary key (DETAILID)
);

comment on table T_LM_DISTRICTDETAIL is
'1-15全国行政区明细表';

comment on column T_LM_DISTRICTDETAIL.DETAILID is
'明细表ID';

comment on column T_LM_DISTRICTDETAIL.ADDRESSCODE is
'地址编号';

comment on column T_LM_DISTRICTDETAIL.ADDRESSNAME is
'地址名称';

comment on column T_LM_DISTRICTDETAIL.PARENTLDID is
'父级ID';

comment on column T_LM_DISTRICTDETAIL.BESTROWAREA is
'是否是覆盖整个区域';

comment on column T_LM_DISTRICTDETAIL.DEFINEAREAID is
'自定义区域ID';

comment on column T_LM_DISTRICTDETAIL.DEFINEAREA is
'自定义区域名称';

comment on column T_LM_DISTRICTDETAIL.NODEBOUND is
'业务覆盖区域ID';

comment on column T_LM_DISTRICTDETAIL.AVAILLIMIT is
'时效标准';

comment on column T_LM_DISTRICTDETAIL.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_DISTRICTDETAIL.CREATEDATE is
'CREATEDATE';

comment on column T_LM_DISTRICTDETAIL.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_DISTRICTDETAIL.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_DISTRICTDETAIL.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_DISTRICTDETAIL.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_DISTRICTDETAIL.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_DISTRICTDETAIL.OWNERID is
'OWNERID';

comment on column T_LM_DISTRICTDETAIL.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_DISTRICTDETAIL.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_DISTRICTDETAIL.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_DISTRICTMASTER                                   */
/*==============================================================*/
create table T_LM_DISTRICTMASTER  (
   ALERTID              NVARCHAR2(50)                   not null,
   PROVINCECODE         NVARCHAR2(50)                   not null,
   PROVINCENAME         NVARCHAR2(50)                   not null,
   CITYCODE             NVARCHAR2(50),
   CITYNAME             NVARCHAR2(50),
   COUNTYCODE           NVARCHAR2(50),
   COUNTYNAME           NVARCHAR2(50),
   BESTROWAREA          NVARCHAR2(50),
   DEFINEAREAID         NVARCHAR2(50),
   DEFINEAREA           NVARCHAR2(50),
   NODEBOUND            NVARCHAR2(50),
   AVAILLIMIT           NVARCHAR2(50),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_DISTRICTMASTER primary key (ALERTID)
);

comment on table T_LM_DISTRICTMASTER is
'1-14全国行政区主表';

comment on column T_LM_DISTRICTMASTER.ALERTID is
'主键ID';

comment on column T_LM_DISTRICTMASTER.PROVINCECODE is
'省份编号';

comment on column T_LM_DISTRICTMASTER.PROVINCENAME is
'省份名称';

comment on column T_LM_DISTRICTMASTER.CITYCODE is
'地级市编号';

comment on column T_LM_DISTRICTMASTER.CITYNAME is
'地级市名称';

comment on column T_LM_DISTRICTMASTER.COUNTYCODE is
'区县级编号';

comment on column T_LM_DISTRICTMASTER.COUNTYNAME is
'区县级名称';

comment on column T_LM_DISTRICTMASTER.BESTROWAREA is
'是否是覆盖整个区域';

comment on column T_LM_DISTRICTMASTER.DEFINEAREAID is
'自定义区域ID';

comment on column T_LM_DISTRICTMASTER.DEFINEAREA is
'自定义区域名称';

comment on column T_LM_DISTRICTMASTER.NODEBOUND is
'业务覆盖区域ID';

comment on column T_LM_DISTRICTMASTER.AVAILLIMIT is
'时效标准';

comment on column T_LM_DISTRICTMASTER.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_DISTRICTMASTER.CREATEDATE is
'CREATEDATE';

comment on column T_LM_DISTRICTMASTER.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_DISTRICTMASTER.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_DISTRICTMASTER.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_DISTRICTMASTER.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_DISTRICTMASTER.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_DISTRICTMASTER.OWNERID is
'OWNERID';

comment on column T_LM_DISTRICTMASTER.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_DISTRICTMASTER.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_DISTRICTMASTER.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_FOLLOWINFO                                       */
/*==============================================================*/
create table T_LM_FOLLOWINFO  (
   FOLLOWINFOID         NVARCHAR2(50)                   not null,
   ORDERMASTERID        NVARCHAR2(50)                   not null,
   FOLLOWTIME           DATE                            not null,
   OPERATESITE          NVARCHAR2(50),
   OPERATECOURSE        NVARCHAR2(2000)                 not null,
   FOLLOWSTATE          NUMBER(0)                      default 1 not null,
   OPERATEORGCODE       NVARCHAR2(50)                   not null,
   ISALERT              NUMBER(1),
   OPERATEACCOUNTS      NVARCHAR2(50)                   not null,
   REMARK               NVARCHAR2(2000),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_FOLLOWINFO primary key (FOLLOWINFOID)
);

comment on table T_LM_FOLLOWINFO is
'1-10在途追踪表';

comment on column T_LM_FOLLOWINFO.FOLLOWINFOID is
'在途追踪ID';

comment on column T_LM_FOLLOWINFO.ORDERMASTERID is
'订单编号';

comment on column T_LM_FOLLOWINFO.FOLLOWTIME is
'跟踪时间';

comment on column T_LM_FOLLOWINFO.OPERATESITE is
'操作地点';

comment on column T_LM_FOLLOWINFO.OPERATECOURSE is
'跟踪内容';

comment on column T_LM_FOLLOWINFO.FOLLOWSTATE is
'追踪状态';

comment on column T_LM_FOLLOWINFO.OPERATEORGCODE is
'操作机构';

comment on column T_LM_FOLLOWINFO.ISALERT is
'是否自动跟踪';

comment on column T_LM_FOLLOWINFO.OPERATEACCOUNTS is
'操作人';

comment on column T_LM_FOLLOWINFO.REMARK is
'备注';

comment on column T_LM_FOLLOWINFO.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_FOLLOWINFO.CREATEDATE is
'CREATEDATE';

comment on column T_LM_FOLLOWINFO.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_FOLLOWINFO.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_FOLLOWINFO.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_FOLLOWINFO.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_FOLLOWINFO.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_FOLLOWINFO.OWNERID is
'OWNERID';

comment on column T_LM_FOLLOWINFO.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_FOLLOWINFO.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_FOLLOWINFO.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_INGREDIENT                                       */
/*==============================================================*/
create table T_LM_INGREDIENT  (
   INGREDIENTID         NVARCHAR2(50)                   not null,
   CUSTOMERCODE         NVARCHAR2(50)                   not null,
   PROJECTCODE          NVARCHAR2(50)                   not null,
   BURSARYID            NVARCHAR2(50)                   not null,
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   constraint PK_T_LM_INGREDIENT primary key (INGREDIENTID)
);

comment on table T_LM_INGREDIENT is
'1-13方案因素表';

comment on column T_LM_INGREDIENT.INGREDIENTID is
'因素ID';

comment on column T_LM_INGREDIENT.CUSTOMERCODE is
'客户代码';

comment on column T_LM_INGREDIENT.PROJECTCODE is
'方案代码';

comment on column T_LM_INGREDIENT.BURSARYID is
'会计科目（收费项）';

comment on column T_LM_INGREDIENT.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_INGREDIENT.CREATEDATE is
'CREATEDATE';

comment on column T_LM_INGREDIENT.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_INGREDIENT.UPDATEDATE is
'UPDATEDATE';

/*==============================================================*/
/* Table: T_LM_NODE                                             */
/*==============================================================*/
create table T_LM_NODE  (
   NODEID               NVARCHAR2(50)                   not null,
   NODECODE             NVARCHAR2(50)                   not null,
   NODENAME             NVARCHAR2(50),
   ORGCODE              NVARCHAR2(50),
   AREACODE             NVARCHAR2(50)                   not null,
   NODETYPE             NUMBER(1)                       not null,
   NODETYPENAME         NVARCHAR2(50),
   NODEKIND             NUMBER(1)                       not null,
   NODEBOUND            NVARCHAR2(50),
   BUSINESSDATE         DATE                            not null,
   BEGINDATE            DATE,
   ENDDATE              DATE,
   NODEADDRESS          NVARCHAR2(50),
   NAME                 NVARCHAR2(50),
   JOB                  NVARCHAR2(50),
   TELEPHONE            NVARCHAR2(50),
   REMARK               NVARCHAR2(500),
   ATTACHID             NVARCHAR2(50)                   not null,
   STATE                NUMBER(1)                       not null,
   CHECKSTATE           NVARCHAR2(50),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_NODE primary key (NODECODE)
);

comment on table T_LM_NODE is
'1-22网点信息维护';

comment on column T_LM_NODE.NODEID is
'网点ID';

comment on column T_LM_NODE.NODECODE is
'网点代码';

comment on column T_LM_NODE.NODENAME is
'网点名称';

comment on column T_LM_NODE.ORGCODE is
'所属分公司';

comment on column T_LM_NODE.AREACODE is
'所属区域';

comment on column T_LM_NODE.NODETYPE is
'网点类型';

comment on column T_LM_NODE.NODETYPENAME is
'网点类型名称';

comment on column T_LM_NODE.NODEKIND is
'网点性质';

comment on column T_LM_NODE.NODEBOUND is
'业务覆盖区域ID';

comment on column T_LM_NODE.BUSINESSDATE is
'网点开始运作时间';

comment on column T_LM_NODE.BEGINDATE is
'协议开始时间';

comment on column T_LM_NODE.ENDDATE is
'协议截止时间';

comment on column T_LM_NODE.NODEADDRESS is
'网点地址';

comment on column T_LM_NODE.NAME is
'负责人';

comment on column T_LM_NODE.JOB is
'职位';

comment on column T_LM_NODE.TELEPHONE is
'联系电话';

comment on column T_LM_NODE.REMARK is
'备注';

comment on column T_LM_NODE.ATTACHID is
'附件ID';

comment on column T_LM_NODE.STATE is
'网点使用状态';

comment on column T_LM_NODE.CHECKSTATE is
'审核状态';

comment on column T_LM_NODE.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_NODE.CREATEDATE is
'CREATEDATE';

comment on column T_LM_NODE.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_NODE.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_NODE.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_NODE.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_NODE.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_NODE.OWNERID is
'OWNERID';

comment on column T_LM_NODE.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_NODE.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_NODE.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_NODEBANK                                         */
/*==============================================================*/
create table T_LM_NODEBANK  (
   ACCOUNTSID           NVARCHAR2(50)                   not null,
   NODENAME             NVARCHAR2(100),
   HOLDER               NVARCHAR2(50),
   IDENTITYCARD         NVARCHAR2(50),
   BANKACCOUNTS         NVARCHAR2(50),
   BANK                 NVARCHAR2(50),
   BANKADDRESS          NVARCHAR2(50)                  default '1',
   USESTATE             NVARCHAR2(50),
   CHECKSTATE           NVARCHAR2(50),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_NODEBANK primary key (ACCOUNTSID)
);

comment on table T_LM_NODEBANK is
'1-23网点银行帐户表';

comment on column T_LM_NODEBANK.ACCOUNTSID is
'客户银行帐号ID';

comment on column T_LM_NODEBANK.NODENAME is
'网点名称';

comment on column T_LM_NODEBANK.HOLDER is
'开户人';

comment on column T_LM_NODEBANK.IDENTITYCARD is
'开户人身份证号';

comment on column T_LM_NODEBANK.BANKACCOUNTS is
'银行帐号';

comment on column T_LM_NODEBANK.BANK is
'开户银行';

comment on column T_LM_NODEBANK.BANKADDRESS is
'开户银行地址';

comment on column T_LM_NODEBANK.USESTATE is
'使用状态';

comment on column T_LM_NODEBANK.CHECKSTATE is
'审核状态';

comment on column T_LM_NODEBANK.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_NODEBANK.CREATEDATE is
'CREATEDATE';

comment on column T_LM_NODEBANK.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_NODEBANK.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_NODEBANK.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_NODEBANK.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_NODEBANK.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_NODEBANK.OWNERID is
'OWNERID';

comment on column T_LM_NODEBANK.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_NODEBANK.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_NODEBANK.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_ORDERDETAIL                                      */
/*==============================================================*/
create table T_LM_ORDERDETAIL  (
   ORDERDETAILID        NVARCHAR2(50)                   not null,
   ORDERDETAILCODE      NVARCHAR2(50)                   not null,
   ORDERTYPE            NUMBER(1)                       not null,
   SENDWAREHOUSEID      NVARCHAR2(50),
   RECIVEWARHOUSEID     NVARCHAR2(50),
   WORKWAREHOUSEID      NVARCHAR2(50),
   SENDERID             NVARCHAR2(50),
   SENDERNAME           NVARCHAR2(100),
   SOURCECUSTOMERID     NVARCHAR2(50),
   SOURCEAREAID         NVARCHAR2(50),
   SOURCEADDRESS        NVARCHAR2(2000),
   SOURCELINKMAN        NVARCHAR2(50),
   SENDERPOSTCODE       NVARCHAR2(50),
   SOURCETEL            NVARCHAR2(50),
   RECEIVERID           NVARCHAR2(50),
   RECEIVERNAME         NVARCHAR2(100),
   RECIVERPOSTCODE      NVARCHAR2(50),
   DESTAREAID           NVARCHAR2(50),
   DESTADDRESS          NVARCHAR2(2000),
   DESTLINKMAN          NVARCHAR2(50),
   DESTTEL              NVARCHAR2(50),
   TASKMODE             NVARCHAR2(50),
   AVAILTIME            DATE,
   AVAILLIMIT           NUMBER,
   AVAILLIMITALERT      NUMBER,
   BILLSTATE            NVARCHAR2(50)                  default '1',
   ACCEPTDATE           DATE                           default SYSDATE,
   ORDERVALUE           NUMBER(10,2)                   default 0,
   TOTALPIECE           NUMBER                         default 0,
   AMOUNT               NUMBER                         default 0,
   TOTALWEIGHT          NUMBER                         default 0,
   盘点时间点                NVARCHAR2(50),
   盘点周期                 NVARCHAR2(50),
   ISCARRYSIGN          NUMBER(1),
   REMARK               NVARCHAR2(2000),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_ORDERDETAIL primary key (ORDERDETAILID)
);

comment on table T_LM_ORDERDETAIL is
'1-6订单明细表';

comment on column T_LM_ORDERDETAIL.ORDERDETAILID is
'单据ID';

comment on column T_LM_ORDERDETAIL.ORDERDETAILCODE is
'单据编号';

comment on column T_LM_ORDERDETAIL.ORDERTYPE is
'单据类别';

comment on column T_LM_ORDERDETAIL.SENDWAREHOUSEID is
'发货仓库ID';

comment on column T_LM_ORDERDETAIL.RECIVEWARHOUSEID is
'入库仓库ID';

comment on column T_LM_ORDERDETAIL.WORKWAREHOUSEID is
'作业仓库ID';

comment on column T_LM_ORDERDETAIL.SENDERID is
'发货客户ID';

comment on column T_LM_ORDERDETAIL.SENDERNAME is
'发货客户名称';

comment on column T_LM_ORDERDETAIL.SOURCECUSTOMERID is
'货源地客户ID';

comment on column T_LM_ORDERDETAIL.SOURCEAREAID is
'货源地所属区域ID';

comment on column T_LM_ORDERDETAIL.SOURCEADDRESS is
'货源地地址';

comment on column T_LM_ORDERDETAIL.SOURCELINKMAN is
'货源地联系人';

comment on column T_LM_ORDERDETAIL.SENDERPOSTCODE is
'发货人邮编';

comment on column T_LM_ORDERDETAIL.SOURCETEL is
'货源地电话';

comment on column T_LM_ORDERDETAIL.RECEIVERID is
'收货客户ID';

comment on column T_LM_ORDERDETAIL.RECEIVERNAME is
'收货客户名称';

comment on column T_LM_ORDERDETAIL.RECIVERPOSTCODE is
'收货人邮编';

comment on column T_LM_ORDERDETAIL.DESTAREAID is
'目的地所属区域ID';

comment on column T_LM_ORDERDETAIL.DESTADDRESS is
'目的地址';

comment on column T_LM_ORDERDETAIL.DESTLINKMAN is
'目的地联系人';

comment on column T_LM_ORDERDETAIL.DESTTEL is
'目的地电话';

comment on column T_LM_ORDERDETAIL.TASKMODE is
'单据作业方式';

comment on column T_LM_ORDERDETAIL.AVAILTIME is
'时效起始时间';

comment on column T_LM_ORDERDETAIL.AVAILLIMIT is
'时效标准';

comment on column T_LM_ORDERDETAIL.AVAILLIMITALERT is
'时效警戒线';

comment on column T_LM_ORDERDETAIL.BILLSTATE is
'单据状态';

comment on column T_LM_ORDERDETAIL.ACCEPTDATE is
'预约时间';

comment on column T_LM_ORDERDETAIL.ORDERVALUE is
'单据费用';

comment on column T_LM_ORDERDETAIL.TOTALPIECE is
'件数';

comment on column T_LM_ORDERDETAIL.AMOUNT is
'数量';

comment on column T_LM_ORDERDETAIL.TOTALWEIGHT is
'重量';

comment on column T_LM_ORDERDETAIL.盘点时间点 is
'盘点时间点';

comment on column T_LM_ORDERDETAIL.盘点周期 is
'盘点周期';

comment on column T_LM_ORDERDETAIL.ISCARRYSIGN is
'是否回执';

comment on column T_LM_ORDERDETAIL.REMARK is
'备注';

comment on column T_LM_ORDERDETAIL.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_ORDERDETAIL.CREATEDATE is
'CREATEDATE';

comment on column T_LM_ORDERDETAIL.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_ORDERDETAIL.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_ORDERDETAIL.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_ORDERDETAIL.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_ORDERDETAIL.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_ORDERDETAIL.OWNERID is
'OWNERID';

comment on column T_LM_ORDERDETAIL.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_ORDERDETAIL.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_ORDERDETAIL.CREATEPOSTID is
'CREATEPOSTID';

--WARNING: The following insert order will fail because it cannot give value to mandatory columns
insert into T_LM_ORDERDETAIL (ORDERDETAILID, ORDERDETAILCODE, ORDERTYPE, CREATEUSERID, CREATEDATE, UPDATEUSERID, UPDATEDATE)
select ORDERDETAILID, ?, ?, CREATEUSERID, CREATEDATE, UPDATEUSERID, UPDATEDATE
from SMTLM."tmp_T_LM_ORDERDETAIL";

/*==============================================================*/
/* Table: T_LM_ORDERDMASTER                                     */
/*==============================================================*/
create table T_LM_ORDERDMASTER  (
   ORDERMASTERID        NVARCHAR2(50)                   not null,
   ORDERMASTERCODE      NVARCHAR2(50)                   not null,
   CUSTOMERDICTATE      NVARCHAR2(50)                   not null,
   ORDERSOURCE          NUMBER(1)                       not null,
   ORDERTYPE            NUMBER(1)                       not null,
   ORDERTYPENAME        NVARCHAR2(100),
   ORGANIZATIONID       NVARCHAR2(50)                   not null,
   ORGANIZATIONNAME     NVARCHAR2(100),
   CONSIGNERID          NVARCHAR2(50)                   not null,
   CONSIGNERNAME        NVARCHAR2(100),
   CHARGEMODE           NUMBER(1),
   ORDERVALUE           NUMBER(10,2)                   default 0,
   TOTALPIECE           NUMBER                         default 0,
   AMOUNT               NUMBER                         default 0,
   TOTALWEIGHT          NUMBER                         default 0,
   REMARK               NVARCHAR2(2000),
   CHECKSTATE           NVARCHAR2(50),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_ORDERDMASTER primary key (ORDERMASTERID)
);

comment on table T_LM_ORDERDMASTER is
'1-5订单主表';

comment on column T_LM_ORDERDMASTER.ORDERMASTERID is
'订单ID';

comment on column T_LM_ORDERDMASTER.ORDERMASTERCODE is
'订单编号';

comment on column T_LM_ORDERDMASTER.CUSTOMERDICTATE is
'客户指令编码';

comment on column T_LM_ORDERDMASTER.ORDERSOURCE is
'订单来源类别';

comment on column T_LM_ORDERDMASTER.ORDERTYPE is
'订单类别';

comment on column T_LM_ORDERDMASTER.ORDERTYPENAME is
'订单类别名称';

comment on column T_LM_ORDERDMASTER.ORGANIZATIONID is
'机构ID';

comment on column T_LM_ORDERDMASTER.ORGANIZATIONNAME is
'机构名称';

comment on column T_LM_ORDERDMASTER.CONSIGNERID is
'委托客户ID';

comment on column T_LM_ORDERDMASTER.CONSIGNERNAME is
'客户名称';

comment on column T_LM_ORDERDMASTER.CHARGEMODE is
'收费方式';

comment on column T_LM_ORDERDMASTER.ORDERVALUE is
'订单总费用';

comment on column T_LM_ORDERDMASTER.TOTALPIECE is
'件数';

comment on column T_LM_ORDERDMASTER.AMOUNT is
'总数量';

comment on column T_LM_ORDERDMASTER.TOTALWEIGHT is
'总重量';

comment on column T_LM_ORDERDMASTER.REMARK is
'备注';

comment on column T_LM_ORDERDMASTER.CHECKSTATE is
'审核状态';

comment on column T_LM_ORDERDMASTER.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_ORDERDMASTER.CREATEDATE is
'CREATEDATE';

comment on column T_LM_ORDERDMASTER.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_ORDERDMASTER.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_ORDERDMASTER.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_ORDERDMASTER.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_ORDERDMASTER.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_ORDERDMASTER.OWNERID is
'OWNERID';

comment on column T_LM_ORDERDMASTER.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_ORDERDMASTER.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_ORDERDMASTER.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_ORDERGOODS                                       */
/*==============================================================*/
create table T_LM_ORDERGOODS  (
   ORDERGOODSID         NVARCHAR2(50)                   not null,
   订单流水号                NVARCHAR2(50),
   GOODSTYPEID          NVARCHAR2(50),
   GOODSID              NVARCHAR2(50)                   not null,
   GOODSNAME            NVARCHAR2(50)                   not null,
   GOODSSTATE           NVARCHAR2(50)                   not null,
   GOODSQTY             NUMBER,
   ISSCAN               NVARCHAR2(50),
   UNITSID              NVARCHAR2(50),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_ORDERGOODS primary key (ORDERGOODSID)
);

comment on table T_LM_ORDERGOODS is
'1-9订单货品列表';

comment on column T_LM_ORDERGOODS.ORDERGOODSID is
'订单货品明细ID';

comment on column T_LM_ORDERGOODS.订单流水号 is
'订单流水号';

comment on column T_LM_ORDERGOODS.GOODSTYPEID is
'货品类别';

comment on column T_LM_ORDERGOODS.GOODSID is
'货品编码';

comment on column T_LM_ORDERGOODS.GOODSNAME is
'货品名称';

comment on column T_LM_ORDERGOODS.GOODSSTATE is
'货品状态';

comment on column T_LM_ORDERGOODS.GOODSQTY is
'货品数量';

comment on column T_LM_ORDERGOODS.ISSCAN is
'是否扫描';

comment on column T_LM_ORDERGOODS.UNITSID is
'货品单位';

comment on column T_LM_ORDERGOODS.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_ORDERGOODS.CREATEDATE is
'CREATEDATE';

comment on column T_LM_ORDERGOODS.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_ORDERGOODS.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_ORDERGOODS.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_ORDERGOODS.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_ORDERGOODS.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_ORDERGOODS.OWNERID is
'OWNERID';

comment on column T_LM_ORDERGOODS.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_ORDERGOODS.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_ORDERGOODS.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_ORDERIMEI                                        */
/*==============================================================*/
create table T_LM_ORDERIMEI  (
   ORDERIMEIID          NVARCHAR2(50)                   not null,
   IMEI                 NVARCHAR2(50),
   constraint PK_T_LM_ORDERIMEI primary key (ORDERIMEIID)
);

comment on table T_LM_ORDERIMEI is
'1-11订单货品串号表';

comment on column T_LM_ORDERIMEI.ORDERIMEIID is
'串号ID';

comment on column T_LM_ORDERIMEI.IMEI is
'串号';

/*==============================================================*/
/* Table: T_LM_ORDERREQUEST                                     */
/*==============================================================*/
create table T_LM_ORDERREQUEST  (
   ORDERREQUESTID       NVARCHAR2(50)                   not null,
   ORDERDETAILID        NVARCHAR2(50)                   not null,
   ORDERSN              NVARCHAR2(50),
   REFERRENCE           NUMBER                          not null,
   PAYMENTPRICE         NUMBER(10,2),
   BURSARYID            NVARCHAR2(50)                   not null,
   REMARK               NVARCHAR2(2000),
   CHECKSTATE           NVARCHAR2(50),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_ORDERREQUEST primary key (ORDERREQUESTID)
);

comment on table T_LM_ORDERREQUEST is
'1-7订单收费项';

comment on column T_LM_ORDERREQUEST.ORDERREQUESTID is
'收费项ID';

comment on column T_LM_ORDERREQUEST.ORDERDETAILID is
'单据ID';

comment on column T_LM_ORDERREQUEST.ORDERSN is
'收费序号';

comment on column T_LM_ORDERREQUEST.REFERRENCE is
'收费参考值';

comment on column T_LM_ORDERREQUEST.PAYMENTPRICE is
'收费价格';

comment on column T_LM_ORDERREQUEST.BURSARYID is
'会计科目（收费项）';

comment on column T_LM_ORDERREQUEST.REMARK is
'备注';

comment on column T_LM_ORDERREQUEST.CHECKSTATE is
'审核状态';

comment on column T_LM_ORDERREQUEST.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_ORDERREQUEST.CREATEDATE is
'CREATEDATE';

comment on column T_LM_ORDERREQUEST.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_ORDERREQUEST.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_ORDERREQUEST.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_ORDERREQUEST.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_ORDERREQUEST.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_ORDERREQUEST.OWNERID is
'OWNERID';

comment on column T_LM_ORDERREQUEST.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_ORDERREQUEST.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_ORDERREQUEST.CREATEPOSTID is
'CREATEPOSTID';

alter table T_LM_PREFIX
   add constraint PK_T_LM_PREFIX primary key (PREFIXTYPEID, PREFIXID);

/*==============================================================*/
/* Table: T_LM_REFUSEORDER                                      */
/*==============================================================*/
create table T_LM_REFUSEORDER  (
   REFUSEORDERID        NVARCHAR2(50)                   not null,
   ORDERMASTERCODE      NVARCHAR2(50)                   not null,
   DEFINEAREAID         NVARCHAR2(50),
   CUSTOMERDICTATE      NVARCHAR2(50)                   not null,
   ORDERSOURCE          NUMBER(1)                       not null,
   ORDERTYPE            NUMBER(1)                       not null,
   ORGANIZATIONID       NVARCHAR2(50)                   not null,
   CONSIGNERID          NVARCHAR2(50)                   not null,
   客户名称                 NVARCHAR2(50),
   操作用户                 NVARCHAR2(50),
   拒绝日期                 NVARCHAR2(50),
   REMARK               NVARCHAR2(2000),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_REFUSEORDER primary key (REFUSEORDERID)
);

comment on table T_LM_REFUSEORDER is
'1-16被拒绝的单据';

comment on column T_LM_REFUSEORDER.REFUSEORDERID is
'被拒绝的订单ID';

comment on column T_LM_REFUSEORDER.ORDERMASTERCODE is
'订单编号';

comment on column T_LM_REFUSEORDER.DEFINEAREAID is
'服务区域ID';

comment on column T_LM_REFUSEORDER.CUSTOMERDICTATE is
'客户指令编码';

comment on column T_LM_REFUSEORDER.ORDERSOURCE is
'订单来源类别';

comment on column T_LM_REFUSEORDER.ORDERTYPE is
'订单类别';

comment on column T_LM_REFUSEORDER.ORGANIZATIONID is
'机构ID';

comment on column T_LM_REFUSEORDER.CONSIGNERID is
'委托客户ID';

comment on column T_LM_REFUSEORDER.客户名称 is
'客户名称';

comment on column T_LM_REFUSEORDER.操作用户 is
'操作用户';

comment on column T_LM_REFUSEORDER.拒绝日期 is
'拒绝日期';

comment on column T_LM_REFUSEORDER.REMARK is
'备注';

comment on column T_LM_REFUSEORDER.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_REFUSEORDER.CREATEDATE is
'CREATEDATE';

comment on column T_LM_REFUSEORDER.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_REFUSEORDER.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_REFUSEORDER.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_REFUSEORDER.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_REFUSEORDER.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_REFUSEORDER.OWNERID is
'OWNERID';

comment on column T_LM_REFUSEORDER.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_REFUSEORDER.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_REFUSEORDER.CREATEPOSTID is
'CREATEPOSTID';

/*==============================================================*/
/* Table: T_LM_SALESMAN                                         */
/*==============================================================*/
create table T_LM_SALESMAN  (
   PERSONNELID          NVARCHAR2(50),
   SMCODE               NVARCHAR2(50)                   not null,
   NODENAME             NVARCHAR2(100),
   SMNAME               NVARCHAR2(50)                   not null,
   SEX                  NUMBER(1)                      default 3 not null,
   IDCARDNO             NVARCHAR2(50),
   PHONE                NVARCHAR2(100),
   MOBILE               NVARCHAR2(50),
   HABITATCITY          NVARCHAR2(50),
   HABITATION           NVARCHAR2(50),
   STATE                NUMBER                         default 1 not null,
   INDATE               DATE                            not null,
   SMTYPE               NUMBER(1)                      default 1 not null,
   REMARK               NVARCHAR2(2000),
   CHECKSTATE           NVARCHAR2(50),
   CREATEUSERID         NVARCHAR2(50),
   CREATEDATE           DATE                           default SYSDATE not null,
   UPDATEUSERID         NVARCHAR2(50),
   UPDATEDATE           DATE                           default SYSDATE not null,
   OWNERCOMPANYID       NVARCHAR2(50),
   OWNERDEPARTMENTID    NVARCHAR2(50),
   OWNERPOSTID          NVARCHAR2(50),
   OWNERID              NVARCHAR2(50),
   CREATECOMPANYID      NVARCHAR2(50),
   CREATEDEPARTMENTID   NVARCHAR2(50),
   CREATEPOSTID         NVARCHAR2(50),
   constraint PK_T_LM_SALESMAN primary key (SMCODE)
);

comment on table T_LM_SALESMAN is
'1-24网点人员信息表';

comment on column T_LM_SALESMAN.PERSONNELID is
'人员信息ID';

comment on column T_LM_SALESMAN.SMCODE is
'人员编号';

comment on column T_LM_SALESMAN.NODENAME is
'网点名称';

comment on column T_LM_SALESMAN.SMNAME is
'人员姓名';

comment on column T_LM_SALESMAN.SEX is
'性别';

comment on column T_LM_SALESMAN.IDCARDNO is
'身份证号';

comment on column T_LM_SALESMAN.PHONE is
'联络电话';

comment on column T_LM_SALESMAN.MOBILE is
'手机号码';

comment on column T_LM_SALESMAN.HABITATCITY is
'居住地城市';

comment on column T_LM_SALESMAN.HABITATION is
'居住地';

comment on column T_LM_SALESMAN.STATE is
'人员状态';

comment on column T_LM_SALESMAN.INDATE is
'入职时间';

comment on column T_LM_SALESMAN.SMTYPE is
'人员属性';

comment on column T_LM_SALESMAN.REMARK is
'备注';

comment on column T_LM_SALESMAN.CHECKSTATE is
'审核状态';

comment on column T_LM_SALESMAN.CREATEUSERID is
'CREATEUSERID';

comment on column T_LM_SALESMAN.CREATEDATE is
'CREATEDATE';

comment on column T_LM_SALESMAN.UPDATEUSERID is
'UPDATEUSERID';

comment on column T_LM_SALESMAN.UPDATEDATE is
'UPDATEDATE';

comment on column T_LM_SALESMAN.OWNERCOMPANYID is
'OWNERCOMPANYID';

comment on column T_LM_SALESMAN.OWNERDEPARTMENTID is
'OWNERDEPARTMENTID';

comment on column T_LM_SALESMAN.OWNERPOSTID is
'OWNERPOSTID';

comment on column T_LM_SALESMAN.OWNERID is
'OWNERID';

comment on column T_LM_SALESMAN.CREATECOMPANYID is
'CREATECOMPANYID';

comment on column T_LM_SALESMAN.CREATEDEPARTMENTID is
'CREATEDEPARTMENTID';

comment on column T_LM_SALESMAN.CREATEPOSTID is
'CREATEPOSTID';

