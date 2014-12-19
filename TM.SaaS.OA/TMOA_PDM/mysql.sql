/*==============================================================*/
/* Table: FLOW_CONSULTATION_T                                   */
/*==============================================================*/
CREATE TABLE FLOW_CONSULTATION_T
(
   CONSULTATIONID       VARCHAR(50) NOT NULL,
   FLOWRECORDDETAILID   VARCHAR(50),
   CONSULTATIONUSERID   VARCHAR(50),
   CONSULTATIONUSERNAME VARCHAR(200),
   CONSULTATIONCONTENT  VARCHAR(200),
   CONSULTATIONDATE     DATETIME,
   REPLYUSERID          VARCHAR(50),
   REPLYUSERNAME        VARCHAR(200),
   REPLYCONTENT         VARCHAR(200),
   REPLYDATE            DATETIME,
   FLAG                 VARCHAR(50) COMMENT '0未回复，1回复',
   PRIMARY KEY (CONSULTATIONID)
);

ALTER TABLE FLOW_CONSULTATION_T COMMENT '咨询';

/*==============================================================*/
/* Table: FLOW_EXCEPTIONLOG                                     */
/*==============================================================*/
CREATE TABLE FLOW_EXCEPTIONLOG
(
   ID                   VARCHAR(50) NOT NULL COMMENT '主键ID',
   FORMID               VARCHAR(50) COMMENT '业务ID',
   MODELCODE            VARCHAR(50) COMMENT '模块代码',
   STATE                VARCHAR(50) DEFAULT '未处理' COMMENT '状态:未处理;已处理',
   CREATEDATE           DATETIME  COMMENT '创建日期',
   CREATENAME           VARCHAR(50) COMMENT '创建人',
   UPDATEDATE           DATETIME COMMENT '处理日期',
   UPDATENAME           VARCHAR(50) COMMENT '处理人',
   REMARK               VARCHAR(2000) COMMENT '备注',
   SUBMITINFO           VARCHAR(2000) COMMENT '提交信息',
   LOGINFO              TEXT COMMENT '异常日志信息',
   MODELNAME            VARCHAR(50) COMMENT '模块名称',
   OWNERID              VARCHAR(50) COMMENT '审核人ID',
   OWNERNAME            VARCHAR(50) COMMENT '审核人姓名',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '审核人公司ID',
   OWNERCOMPANYNAME     VARCHAR(50) COMMENT '审核人公司名称',
   OWNERDEPARMENTID     VARCHAR(50) COMMENT '审核人部门ID',
   OWNERDEPARMENTNAME   VARCHAR(50) COMMENT '审核人部门名称',
   OWNERPOSTID          VARCHAR(50) COMMENT '审核人岗位ID',
   OWNERPOSTNAME        VARCHAR(50) COMMENT '审核人岗位名称',
   AUDITSTATE           VARCHAR(50) COMMENT '审核状态;审核通过,审核不通过',
   PRIMARY KEY (ID)
);

ALTER TABLE FLOW_EXCEPTIONLOG COMMENT '异常记录日志';

/*==============================================================*/
/* Index: INDEX_LOG                                             */
/*==============================================================*/
CREATE INDEX INDEX_LOG ON FLOW_EXCEPTIONLOG
(
   CREATEDATE
);

/*==============================================================*/
/* Table: FLOW_FLOWCATEGORY                                     */
/*==============================================================*/
CREATE TABLE FLOW_FLOWCATEGORY
(
   FLOWCATEGORYID       VARCHAR(50) NOT NULL COMMENT '流程分类ID',
   FLOWCATEGORYDESC     VARCHAR(50) NOT NULL COMMENT '流程分类描述',
   COMPANYID            VARCHAR(50) COMMENT '所属公司',
   PRIMARY KEY (FLOWCATEGORYID)
);

ALTER TABLE FLOW_FLOWCATEGORY COMMENT '流程分类表';

/*==============================================================*/
/* Table: FLOW_FLOWDEFINE_AUDIT_T                               */
/*==============================================================*/
CREATE TABLE FLOW_FLOWDEFINE_AUDIT_T
(
   FLOWDEFINEID         VARCHAR(50) NOT NULL COMMENT '流程定义ID',
   FLOWCODE             VARCHAR(50) NOT NULL COMMENT '流程代码',
   DESCRIPTION          VARCHAR(50) NOT NULL COMMENT '名称描述',
   XOML                 TEXT NOT NULL COMMENT '模型文件',
   RULES                TEXT COMMENT '模型规则',
   LAYOUT               TEXT NOT NULL COMMENT '模型布局',
   FLOWTYPE             VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '流程类型 -- 0:审批流程, 1:任务流程',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人ID',
   CREATEUSERNAME       VARCHAR(50) NOT NULL COMMENT '创建人名',
   CREATECOMPANYID      VARCHAR(50) NOT NULL COMMENT '创建公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL COMMENT '创建部门ID',
   CREATEPOSTID         VARCHAR(50) NOT NULL COMMENT '创建岗位ID',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建时间',
   EDITUSERID           VARCHAR(50) COMMENT '修改人ID',
   EDITUSERNAME         VARCHAR(50) COMMENT '修改人用户名',
   EDITDATE             DATETIME COMMENT '修改时间',
   WFLAYOUT             TEXT COMMENT '流程定义文件',
   OWNERID              VARCHAR(50),
   SYSTEMCODE           VARCHAR(50) COMMENT '业务系统:OA,HR,TM等',
   MODELCODETYPE        VARCHAR(50) COMMENT '模块类型：现只针对模块类型',
   MODELCODETYPENAME    VARCHAR(100) COMMENT '模块类型名称：现只针对事项审批类型',
   FLOWCODE1            VARCHAR(50),
   EDITSTATE            VARCHAR(1),
   CHECKSTATES          VARCHAR(1),
   BUSINESSOBJECT       VARCHAR(50) COMMENT '业务对象：各种申请报销单',
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(100),
   OWNERPOSTNAME        VARCHAR(100),
   OWNERNAME            VARCHAR(100),
   OWNERCOMPANYID       VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(100),
   HISTORYSTATE         NUMERIC(38,0) COMMENT '此流程是否启用过。0：未启用，1：启用过
            配合版本号在流程列表中显示状态名称
            版本号=0 and 历史状态=0则状态为修改中。
            版本号=0 and 历史状态=1则状态为启用中
            当审核通过时将versionid=0的数据的HISTORYSTATE设置为1',
   VERSIONNO            NUMERIC(38,0) COMMENT '版本号：默认为0，每新建一个将flocode相同的流程版本号+1
            流程列表获取数据时只取版本号为0的数据。',
   MODELCODE            VARCHAR(50),
   COMPANYID            VARCHAR(50),
   DEPARTMENTID         VARCHAR(50),
   COMPANYNAME          VARCHAR(100),
   DEPARTMENTNAME       VARCHAR(100),
   REASON               VARCHAR(1000),
   CONTENT              VARCHAR(2000),
   PRIMARY KEY (FLOWDEFINEID)
);

ALTER TABLE FLOW_FLOWDEFINE_AUDIT_T COMMENT '流程定义审核表:流程设计时操作这张表，审核通过后将数据同步到
Flow_FlowDefine_T，如果不需要';

/*==============================================================*/
/* Table: FLOW_FLOWDEFINE_T                                     */
/*==============================================================*/
CREATE TABLE FLOW_FLOWDEFINE_T
(
   FLOWDEFINEID         VARCHAR(50) NOT NULL COMMENT '流程定义ID',
   FLOWCODE             VARCHAR(50) NOT NULL COMMENT '流程代码',
   DESCRIPTION          VARCHAR(50) NOT NULL COMMENT '名称描述',
   XOML                 TEXT NOT NULL COMMENT '模型文件',
   RULES                TEXT COMMENT '模型规则',
   LAYOUT               TEXT NOT NULL COMMENT '模型布局',
   FLOWTYPE             VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '流程类型 -- 0:审批流程, 1:任务流程',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人ID',
   CREATEUSERNAME       VARCHAR(50) NOT NULL COMMENT '创建人名',
   CREATECOMPANYID      VARCHAR(50) NOT NULL COMMENT '创建公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL COMMENT '创建部门ID',
   CREATEPOSTID         VARCHAR(50) NOT NULL COMMENT '创建岗位ID',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建时间',
   EDITUSERID           VARCHAR(50) COMMENT '修改人ID',
   EDITUSERNAME         VARCHAR(50) COMMENT '修改人用户名',
   EDITDATE             DATETIME COMMENT '修改时间',
   SYSTEMCODE           VARCHAR(50) COMMENT '业务系统:OA,HR,TM等',
   BUSINESSOBJECT       VARCHAR(50) COMMENT '业务对象：各种申请报销单',
   WFLAYOUT             TEXT COMMENT '流程定义文件',
   FLOWCODE1            VARCHAR(50),
   MODELCODETYPE        VARCHAR(50) COMMENT '模块类型：现在只
            针对事项审批类型',
   MODELCODETYPENAME    VARCHAR(100) COMMENT '模块类型名
            称：现在只针对事项审批类型',
   REFFLOWDEFINEID      VARCHAR(50) COMMENT '对应审核表的流程ID',
   PRIMARY KEY (FLOWCODE)
);

ALTER TABLE FLOW_FLOWDEFINE_T COMMENT '流程模型定义表';

/*==============================================================*/
/* Index: IDX_BUSOBJ                                            */
/*==============================================================*/
CREATE INDEX IDX_BUSOBJ ON FLOW_FLOWDEFINE_T
(
   BUSINESSOBJECT
);

/*==============================================================*/
/* Table: FLOW_FLOWDEFINE_T_HISTORY                             */
/*==============================================================*/
CREATE TABLE FLOW_FLOWDEFINE_T_HISTORY
(
   FLOWDEFINEID         VARCHAR(50) NOT NULL,
   FLOWCODE             VARCHAR(200) NOT NULL,
   DESCRIPTION          VARCHAR(100) NOT NULL,
   XOML                 TEXT NOT NULL,
   RULES                TEXT,
   LAYOUT               TEXT NOT NULL,
   FLOWTYPE             VARCHAR(1) NOT NULL DEFAULT '0',
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   EDITUSERID           VARCHAR(50),
   EDITUSERNAME         VARCHAR(50),
   EDITDATE             DATETIME,
   SYSTEMCODE           VARCHAR(50),
   BUSINESSOBJECT       VARCHAR(50),
   WFLAYOUT             TEXT,
   FLOWCODE1            VARCHAR(50),
   MODELCODETYPE        VARCHAR(50) COMMENT '模块类
            型：现在只针对事项审批类型',
   MODELCODETYPENAME    VARCHAR(100) COMMENT '模块
            类型名称：现在只针对事项审批类型',
   PRIMARY KEY (FLOWDEFINEID)
);

ALTER TABLE FLOW_FLOWDEFINE_T_HISTORY COMMENT '流程模型历史定义表';

/*==============================================================*/
/* Table: FLOW_FLOWRECORDDETAIL_T                               */
/*==============================================================*/
CREATE TABLE FLOW_FLOWRECORDDETAIL_T
(
   FLOWRECORDDETAILID   VARCHAR(50) NOT NULL,
   FLOWRECORDMASTERID   VARCHAR(50) NOT NULL,
   STATECODE            VARCHAR(50) NOT NULL,
   PARENTSTATEID        VARCHAR(50) NOT NULL,
   CONTENT              VARCHAR(2000),
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '2' COMMENT '0：不同意  1：同意  2：未处理  7：会签同意  8：会签不同意',
   FLAG                 VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未审批  1：已审批',
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   EDITUSERID           VARCHAR(50) NOT NULL,
   EDITUSERNAME         VARCHAR(50) NOT NULL,
   EDITCOMPANYID        VARCHAR(50),
   EDITDEPARTMENTID     VARCHAR(50),
   EDITPOSTID           VARCHAR(50),
   EDITDATE             DATETIME,
   AGENTUSERID          VARCHAR(50),
   AGENTERNAME          VARCHAR(50),
   AGENTEDITDATE        DATETIME,
   PRIMARY KEY (FLOWRECORDDETAILID)
);

ALTER TABLE FLOW_FLOWRECORDDETAIL_T COMMENT '流程审批明细表';

/*==============================================================*/
/* Index: FLCORDFGEDID                                          */
/*==============================================================*/
CREATE INDEX FLCORDFGEDID ON FLOW_FLOWRECORDDETAIL_T
(
   FLAG,
   EDITUSERID,
   EDITPOSTID
);

/*==============================================================*/
/* Table: FLOW_FLOWRECORDMASTER_T                               */
/*==============================================================*/
CREATE TABLE FLOW_FLOWRECORDMASTER_T
(
   FLOWRECORDMASTERID   VARCHAR(50) NOT NULL,
   INSTANCEID           VARCHAR(50) NOT NULL,
   FLOWSELECTTYPE       VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:固定流程，1：自选流程',
   MODELCODE            VARCHAR(50) NOT NULL,
   FLOWCODE             VARCHAR(50) NOT NULL,
   FORMID               VARCHAR(50) NOT NULL,
   FLOWTYPE             VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:审批流程，1：任务流程',
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '1' COMMENT '1:审批中，2：审批通过，3审批不通过，5撤销(为与字典保持一致)',
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   EDITUSERID           VARCHAR(50) NOT NULL,
   EDITUSERNAME         VARCHAR(50) NOT NULL,
   EDITDATE             DATETIME,
   ACTIVEROLE           TEXT,
   BUSINESSOBJECT       TEXT,
   KPITIMEXML           TEXT,
   BUSINESSOBJECT2      TEXT,
   MONTH2               NUMERIC(8,0),
   OWNERUSERID          VARCHAR(50) COMMENT '单据所属人ID',
   OWNERUSERNAME        VARCHAR(50) COMMENT '单据所属人姓名',
   PRIMARY KEY (FLOWRECORDMASTERID)
);

ALTER TABLE FLOW_FLOWRECORDMASTER_T COMMENT '流程审批实例表';

/*==============================================================*/
/* Index: IDX_FLOWRECORDMASTER_MODFOR                           */
/*==============================================================*/
CREATE INDEX IDX_FLOWRECORDMASTER_MODFOR ON FLOW_FLOWRECORDMASTER_T
(
   FORMID
);

/*==============================================================*/
/* Index: IDX_FLOWRE_CHECRE                                     */
/*==============================================================*/
CREATE INDEX IDX_FLOWRE_CHECRE ON FLOW_FLOWRECORDMASTER_T
(
   CHECKSTATE
);

/*==============================================================*/
/* Table: FLOW_FLOWTOCATEGORY                                   */
/*==============================================================*/
CREATE TABLE FLOW_FLOWTOCATEGORY
(
   FLOWCATEGORYID       VARCHAR(50) NOT NULL COMMENT '流程分类ID',
   FLOWCODE             VARCHAR(50) NOT NULL COMMENT '流程代码',
   PRIMARY KEY (FLOWCATEGORYID, FLOWCODE)
);

ALTER TABLE FLOW_FLOWTOCATEGORY COMMENT '流程与分类关系表';

/*==============================================================*/
/* Table: FLOW_FREEFLOWEMPLOYEE                                 */
/*==============================================================*/
CREATE TABLE FLOW_FREEFLOWEMPLOYEE
(
   FREEFLOWID           VARCHAR(50) NOT NULL,
   FORMID               VARCHAR(50) NOT NULL,
   SEQUENCE             NUMERIC(8,0) COMMENT '审核顺序',
   NEXTEDITUSEID        VARCHAR(50) COMMENT '下一审核人ID',
   EDITUSERNAME         VARCHAR(50) NOT NULL COMMENT '审核人姓名',
   NEXTEDITUSERNAME     VARCHAR(50) COMMENT '下一审核人姓名',
   EDITPOSTID           VARCHAR(50) COMMENT '审核人所属岗位名称ID',
   EDITCOMPANYID        VARCHAR(50) COMMENT '审核人所属公司名称ID',
   STATE                VARCHAR(2) DEFAULT '0' COMMENT '处理状态：0未审核，1已审核',
   EDITDEPARTMENTID     VARCHAR(50) COMMENT '审核人所属部门名称ID',
   EDITUSERID           VARCHAR(50) NOT NULL COMMENT '审核人ID',
   CREATEDATE           DATETIME NOT NULL  COMMENT '创建日期',
   ROLENAME             VARCHAR(500) COMMENT '审核人的角色(可能有多个角色)',
   DEPARTMENTNAME       VARCHAR(50) COMMENT '审核人所属部门名称',
   COMPANYNAME          VARCHAR(50) COMMENT '审核人所属公司名称',
   POSTNAME             VARCHAR(50) COMMENT '审核人所属岗位名称',
   FLOWRECORDMASTERID   VARCHAR(50) COMMENT '审核主表主键',
   COUNTERSIGNTYPE      VARCHAR(50) DEFAULT '0' COMMENT '会签类型：【0：所有通过则为通过】【1：一个通过则为通过】【2：通过率】',
   PASSRATE             NUMERIC(8,0) COMMENT '通过率【20表示通过率为20%】',
   BATCH                VARCHAR(50) COMMENT '是否同一批审核人（针对会签）,以时间作为批次如：20140106101015',
   FLOWTYPE             VARCHAR(50) DEFAULT '1' COMMENT '【1：逐级审批】【2：自选流程】',
   PRIMARY KEY (FREEFLOWID)
);

ALTER TABLE FLOW_FREEFLOWEMPLOYEE COMMENT '逐级审批，自选流程';

/*==============================================================*/
/* Table: FLOW_INSTANCE_STATE                                   */
/*==============================================================*/
CREATE TABLE FLOW_INSTANCE_STATE
(
   INSTANCE_ID          CHAR(36) NOT NULL,
   STATE                LONGBLOB,
   STATUS               NUMERIC(9,0) NOT NULL,
   UNLOCKED             NUMERIC(1,0) NOT NULL,
   BLOCKED              NUMERIC(1,0) NOT NULL,
   INFO                 TEXT,
   MODIFIED             DATETIME NOT NULL,
   OWNER_ID             CHAR(36),
   OWNED_UNTIL          DATETIME,
   NEXT_TIMER           DATETIME,
   FORMID               VARCHAR(50),
   CREATEID             VARCHAR(50) COMMENT '创建人ID',
   CREATENAME           VARCHAR(50) COMMENT '创建人姓名',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   EDITID               VARCHAR(50) COMMENT '下一个审核人ID',
   EDITNAME             VARCHAR(50) COMMENT '下一个审核人姓名',
   ID                   VARCHAR(50) NOT NULL,
   PRIMARY KEY (ID)
);

ALTER TABLE FLOW_INSTANCE_STATE COMMENT '流程审核过程中的持久化实例';

/*==============================================================*/
/* Index: FLOW_INSTANCE_STATE_IDX01                             */
/*==============================================================*/
CREATE INDEX FLOW_INSTANCE_STATE_IDX01 ON FLOW_INSTANCE_STATE
(
   NEXT_TIMER,
   STATUS,
   UNLOCKED
);

/*==============================================================*/
/* Index: FLOW_INSTANCE_STATE_IDX02                             */
/*==============================================================*/
CREATE INDEX FLOW_INSTANCE_STATE_IDX02 ON FLOW_INSTANCE_STATE
(
   OWNED_UNTIL,
   OWNER_ID
);

/*==============================================================*/
/* Table: FLOW_MODELDEFINE_FLOWCANCLE                           */
/*==============================================================*/
CREATE TABLE FLOW_MODELDEFINE_FLOWCANCLE
(
   MODELDEFINEFLOWCANCLEID VARCHAR(50) NOT NULL COMMENT 'GUID',
   MODELCODE            VARCHAR(50) COMMENT '模块代码',
   COMPANYNAME          VARCHAR(50) COMMENT '允许提单人撒回流程公司名称',
   COMPANYID            VARCHAR(50) COMMENT '允许提单人撒回流程公司ID',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人ID',
   CREATEUSERNAME       VARCHAR(50) NOT NULL COMMENT '创建人名',
   CREATECOMPANYID      VARCHAR(50) NOT NULL COMMENT '创建公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL COMMENT '创建部门ID',
   CREATEPOSTID         VARCHAR(50) NOT NULL COMMENT '创建岗位ID',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建时间',
   PRIMARY KEY (MODELDEFINEFLOWCANCLEID)
);

ALTER TABLE FLOW_MODELDEFINE_FLOWCANCLE COMMENT '哪些公司在模块中可以允许提单人撒回流程';

/*==============================================================*/
/* Table: FLOW_MODELDEFINE_FREEFLOW                             */
/*==============================================================*/
CREATE TABLE FLOW_MODELDEFINE_FREEFLOW
(
   MODELDEFINEFREEFLOWID VARCHAR(50) NOT NULL COMMENT 'GUID',
   MODELCODE            VARCHAR(50) COMMENT '模块代码',
   COMPANYNAME          VARCHAR(50) COMMENT '允许自选流程公司名称',
   COMPANYID            VARCHAR(50) COMMENT '允许自选流程公司ID',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人ID',
   CREATEUSERNAME       VARCHAR(50) NOT NULL COMMENT '创建人名',
   CREATECOMPANYID      VARCHAR(50) NOT NULL COMMENT '创建公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL COMMENT '创建部门ID',
   CREATEPOSTID         VARCHAR(50) NOT NULL COMMENT '创建岗位ID',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建时间',
   PRIMARY KEY (MODELDEFINEFREEFLOWID)
);

ALTER TABLE FLOW_MODELDEFINE_FREEFLOW COMMENT '哪些公司在模块中可以允许自选流程';

/*==============================================================*/
/* Table: FLOW_MODELDEFINE_T                                    */
/*==============================================================*/
CREATE TABLE FLOW_MODELDEFINE_T
(
   MODELDEFINEID        VARCHAR(50) NOT NULL COMMENT '模块ID',
   SYSTEMCODE           VARCHAR(50) NOT NULL COMMENT '系统代码',
   MODELCODE            VARCHAR(50) NOT NULL COMMENT '模块代码',
   PARENTMODELCODE      VARCHAR(50) COMMENT '上级模块代码',
   DESCRIPTION          VARCHAR(100) NOT NULL COMMENT '模块描述',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人ID',
   CREATEUSERNAME       VARCHAR(50) NOT NULL COMMENT '创建人名',
   CREATECOMPANYID      VARCHAR(50) NOT NULL COMMENT '创建公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL COMMENT '创建部门ID',
   CREATEPOSTID         VARCHAR(50) NOT NULL COMMENT '创建岗位ID',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建时间',
   EDITUSERID           VARCHAR(50) COMMENT '修改人ID',
   EDITUSERNAME         VARCHAR(50) COMMENT '修改人用户名',
   EDITDATE             DATETIME COMMENT '修改时间',
   SYSTEMNAME           VARCHAR(50),
   PRIMARY KEY (MODELCODE)
);

ALTER TABLE FLOW_MODELDEFINE_T COMMENT '模块定义表';

/*==============================================================*/
/* Table: FLOW_MODELFLOWRELATION_T                              */
/*==============================================================*/
CREATE TABLE FLOW_MODELFLOWRELATION_T
(
   MODELFLOWRELATIONID  VARCHAR(50) NOT NULL COMMENT '关联ID',
   COMPANYID            VARCHAR(50) NOT NULL COMMENT '公司ID',
   DEPARTMENTID         VARCHAR(50) COMMENT '部门ID',
   COMPANYNAME          VARCHAR(50) COMMENT '公司名称',
   DEPARTMENTNAME       VARCHAR(50) COMMENT '部门名称',
   SYSTEMCODE           VARCHAR(50) COMMENT '系统代码',
   MODELCODE            VARCHAR(50) NOT NULL COMMENT '模块代码',
   FLOWCODE             VARCHAR(50) NOT NULL COMMENT '流程代码',
   FLAG                 VARCHAR(1) NOT NULL COMMENT '1这可用，0为不可用',
   FLOWTYPE             VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:审批流程，1：任务流程',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人ID',
   CREATEUSERNAME       VARCHAR(50) NOT NULL COMMENT '创建人名',
   CREATECOMPANYID      VARCHAR(50) NOT NULL COMMENT '创建公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL COMMENT '创建部门ID',
   CREATEPOSTID         VARCHAR(50) NOT NULL COMMENT '创建岗位ID',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建时间',
   EDITUSERID           VARCHAR(50) COMMENT '修改人ID',
   EDITUSERNAME         VARCHAR(50) COMMENT '修改人用户名',
   EDITDATE             DATETIME COMMENT '修改时间',
   MODELCODETYPE        VARCHAR(50) COMMENT '模块类
            型：现在只针对事项审批类型',
   MODELCODETYPENAME    VARCHAR(100) COMMENT '模块
            类型名称：现在只针对事项审批类型',
   PRIMARY KEY (MODELFLOWRELATIONID)
);

ALTER TABLE FLOW_MODELFLOWRELATION_T COMMENT '模块与流程定义关联表';

/*==============================================================*/
/* Index: IDX_DPCOCMID                                          */
/*==============================================================*/
CREATE INDEX IDX_DPCOCMID ON FLOW_MODELFLOWRELATION_T
(
   DEPARTMENTID,
   MODELCODE,
   COMPANYID
);

/*==============================================================*/
/* Table: FLOW_MODELFLOWRELATION_T140623                        */
/*==============================================================*/
CREATE TABLE FLOW_MODELFLOWRELATION_T140623
(
   MODELFLOWRELATIONID  VARCHAR(50) NOT NULL,
   COMPANYID            VARCHAR(50) NOT NULL,
   DEPARTMENTID         VARCHAR(50),
   COMPANYNAME          VARCHAR(50),
   DEPARTMENTNAME       VARCHAR(50),
   SYSTEMCODE           VARCHAR(50),
   MODELCODE            VARCHAR(50) NOT NULL,
   FLOWCODE             VARCHAR(50) NOT NULL,
   FLAG                 VARCHAR(1) NOT NULL,
   FLOWTYPE             VARCHAR(1) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   EDITUSERID           VARCHAR(50),
   EDITUSERNAME         VARCHAR(50),
   EDITDATE             DATETIME,
   MODELCODETYPE        VARCHAR(50),
   MODELCODETYPENAME    VARCHAR(100)
);

ALTER TABLE FLOW_MODELFLOWRELATION_T140623 COMMENT 'FLOW_MODELFLOWRELATION_T140623';

/*==============================================================*/
/* Table: FLOW_ROLE                                             */
/*==============================================================*/
CREATE TABLE FLOW_ROLE
(
   FLOWROLEID           VARCHAR(50) NOT NULL COMMENT '流程角色ID',
   ROLEID               VARCHAR(50) COMMENT '角色ID',
   ROLENAME             VARCHAR(50) COMMENT '角色名称',
   FLOWCODE             VARCHAR(50),
   FLOWNAME             VARCHAR(50) COMMENT '流程名称',
   REMARK               VARCHAR(50) COMMENT '备注',
   EDITDATE             DATETIME ,
   PRIMARY KEY (FLOWROLEID)
);

ALTER TABLE FLOW_ROLE COMMENT '流程角色表，一条流程用到哪些角色';

/*==============================================================*/
/* Table: T_FB_BORROWAPPLYDETAIL                                */
/*==============================================================*/
CREATE TABLE T_FB_BORROWAPPLYDETAIL
(
   BORROWAPPLYDETAILID  VARCHAR(50) NOT NULL,
   SUBJECTID            VARCHAR(50),
   BORROWAPPLYMASTERID  VARCHAR(50),
   CHARGETYPE           INT COMMENT '1：个人， 2：公共',
   USABLEMONEY          NUMERIC(8,0),
   REMARK               VARCHAR(200),
   BORROWMONEY          NUMERIC(8,0) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   UNREPAYMONEY         NUMERIC(8,0),
   PRIMARY KEY (BORROWAPPLYDETAILID)
);

ALTER TABLE T_FB_BORROWAPPLYDETAIL COMMENT '借款申请明细';

/*==============================================================*/
/* Table: T_FB_BORROWAPPLYMASTER                                */
/*==============================================================*/
CREATE TABLE T_FB_BORROWAPPLYMASTER
(
   BORROWAPPLYMASTERID  VARCHAR(50) NOT NULL,
   EXTENSIONALORDERID   VARCHAR(50),
   BORROWAPPLYMASTERCODE VARCHAR(50) NOT NULL,
   REPAYTYPE            INT NOT NULL COMMENT '1普通借款
            2备用金借款
            3专项借款
            
            ',
   PLANREPAYDATE        DATETIME,
   TOTALMONEY           NUMERIC(8,0) NOT NULL,
   ISREPAIED            INT COMMENT '0: 还未还清
            1 : 已还清
            ',
   EDITSTATES           INT NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          INT NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   RECEIVER             VARCHAR(50),
   BANK                 VARCHAR(50),
   BANKACCOUT           VARCHAR(50),
   PAYTARGET            INT COMMENT '1 : 付本人
            2 : 付其帐号',
   REMARK               VARCHAR(2000),
   PAYMENTINFO          VARCHAR(2000) COMMENT '支付信息(打印专用)',
   PRIMARY KEY (BORROWAPPLYMASTERID)
);

ALTER TABLE T_FB_BORROWAPPLYMASTER COMMENT '借款申请单';

/*==============================================================*/
/* Table: T_FB_BUDGETACCOUNT                                    */
/*==============================================================*/
CREATE TABLE T_FB_BUDGETACCOUNT
(
   BUDGETACCOUNTID      VARCHAR(50) NOT NULL,
   ACCOUNTOBJECTTYPE    INT COMMENT '1 : 公司, 2 : 部门, 3 : 个人',
   BUDGETYEAR           INT,
   BUDGETMONTH          INT,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   SUBJECTID            VARCHAR(50),
   BUDGETMONEY          NUMERIC(8,0),
   USABLEMONEY          NUMERIC(8,0),
   ACTUALMONEY          NUMERIC(8,0),
   PAIEDMONEY           NUMERIC(8,0),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME,
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME,
   REMARK               VARCHAR(1000),
   CNAME                VARCHAR(2000) COMMENT '公司名',
   DNAME                VARCHAR(2000) COMMENT '部门名',
   PNAME                VARCHAR(2000) COMMENT '岗位名',
   PRIMARY KEY (BUDGETACCOUNTID)
);

ALTER TABLE T_FB_BUDGETACCOUNT COMMENT '预算总账';

/*==============================================================*/
/* Index: INDEX_BUDGETACCOUNT_1                                 */
/*==============================================================*/
CREATE INDEX INDEX_BUDGETACCOUNT_1 ON T_FB_BUDGETACCOUNT
(
   ACCOUNTOBJECTTYPE,
   BUDGETYEAR,
   BUDGETMONTH,
   OWNERCOMPANYID,
   OWNERDEPARTMENTID,
   SUBJECTID,
   OWNERID
);

/*==============================================================*/
/* Index: INDEX_BUDGETACCOUNT_2                                 */
/*==============================================================*/
CREATE INDEX INDEX_BUDGETACCOUNT_2 ON T_FB_BUDGETACCOUNT
(
   ACCOUNTOBJECTTYPE
);

/*==============================================================*/
/* Table: T_FB_BUDGETCHECK                                      */
/*==============================================================*/
CREATE TABLE T_FB_BUDGETCHECK
(
   BUDGETCHECKID        VARCHAR(50) NOT NULL,
   BUDGETCHECKDATE      DATETIME NOT NULL,
   ACCOUNTOBJECTTYPE    INT COMMENT '1 : 公司 , 2 : 部门 ',
   BUDGETYEAR           INT,
   BUDGETMONTH          INT,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50),
   OWNERID              VARCHAR(50),
   SUBJECTID            VARCHAR(50),
   BUDGETMONEY          NUMERIC(8,0),
   ACTUALMONEY          NUMERIC(8,0),
   USABLEMONEY          NUMERIC(8,0),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME,
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME,
   YEARBUDGETMONEY      NUMERIC(8,0),
   YEARTOTALBUDGETMONEY NUMERIC(8,0),
   PRIMARY KEY (BUDGETCHECKID)
);

ALTER TABLE T_FB_BUDGETCHECK COMMENT '预算结算单';

/*==============================================================*/
/* Table: T_FB_CHARGEAPPLYREPAYDETAIL                           */
/*==============================================================*/
CREATE TABLE T_FB_CHARGEAPPLYREPAYDETAIL
(
   CHARGEAPPLYREPAYDETAILID VARCHAR(50) NOT NULL COMMENT '主键',
   CHARGEAPPLYMASTERID  VARCHAR(50) COMMENT '报销单主ID',
   REPAYTYPE            NUMERIC(38,0) NOT NULL COMMENT '1现金还普通借款
            2现金还备用金借款
            3现金还专项借款',
   REMARK               VARCHAR(200) COMMENT '摘要',
   BORROWMONEY          NUMERIC(8,0) COMMENT '借款余额',
   REPAYMONEY           NUMERIC(8,0) NOT NULL COMMENT '冲借款金额',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) NOT NULL COMMENT '修改人',
   UPDATEDATE           DATETIME NOT NULL COMMENT '修改时间',
   PRIMARY KEY (CHARGEAPPLYREPAYDETAILID)
);

ALTER TABLE T_FB_CHARGEAPPLYREPAYDETAIL COMMENT 'T_FB_CHARGEAPPLYREPAYDETAIL';

/*==============================================================*/
/* Table: T_FB_CHARGEAPPLYDETAIL                                */
/*==============================================================*/
CREATE TABLE T_FB_CHARGEAPPLYDETAIL
(
   CHARGEAPPLYDETAILID  VARCHAR(50) NOT NULL,
   SERIALNUMBER         INT,
   SUBJECTID            VARCHAR(50) NOT NULL,
   CHARGEAPPLYMASTERID  VARCHAR(50),
   BORROWAPPLYDETAILID  VARCHAR(50),
   CHARGETYPE           INT COMMENT '1：个人， 2：公共',
   USABLEMONEY          NUMERIC(8,0),
   REPAYMONEY           NUMERIC(8,0),
   REMARK               VARCHAR(200),
   CHARGEMONEY          NUMERIC(8,0) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   PRIMARY KEY (CHARGEAPPLYDETAILID)
);

ALTER TABLE T_FB_CHARGEAPPLYDETAIL COMMENT '费用申请单明细';

/*==============================================================*/
/* Table: T_FB_CHARGEAPPLYMASTER                                */
/*==============================================================*/
CREATE TABLE T_FB_CHARGEAPPLYMASTER
(
   CHARGEAPPLYMASTERID  VARCHAR(50) NOT NULL,
   EXTENSIONALORDERID   VARCHAR(50),
   BORROWAPPLYMASTERID  VARCHAR(50),
   CHARGEAPPLYMASTERCODE VARCHAR(50) NOT NULL,
   BUDGETARYMONTH       DATETIME NOT NULL,
   PAYTYPE              INT NOT NULL COMMENT '1个人费用报销、2冲借款、3冲预付款、4付客户款、5其他',
   REMARK               VARCHAR(2000),
   TOTALMONEY           NUMERIC(8,0) NOT NULL,
   REPAYMENT            NUMERIC(8,0),
   EDITSTATES           INT NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          INT NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   RECEIVER             VARCHAR(50),
   BANK                 VARCHAR(50),
   BANKACCOUT           VARCHAR(50),
   PAYTARGET            INT COMMENT '1 : 付本人
            2 : 付其帐号',
   PAYMENTINFO          VARCHAR(2000) COMMENT '支付信息(打印专用)',
   REPAYTYPE            NUMERIC(38,0) COMMENT '1冲普通借款
            2冲备用金借款',
   ISPAYED              NUMERIC(38,0),
   PRIMARY KEY (CHARGEAPPLYMASTERID)
);

ALTER TABLE T_FB_CHARGEAPPLYMASTER COMMENT '费用申请单';

/*==============================================================*/
/* Index: IDX_CREATEDATESTATUS                                  */
/*==============================================================*/
CREATE INDEX IDX_CREATEDATESTATUS ON T_FB_CHARGEAPPLYMASTER
(
   CREATEDATE,
   CHECKSTATES
);

/*==============================================================*/
/* Table: T_FB_CHARGEBYREPAY                                    */
/*==============================================================*/
CREATE TABLE T_FB_CHARGEBYREPAY
(
   CHARGEBYREPAYID      VARCHAR(50) NOT NULL COMMENT '冲借款单ID',
   CHARGEAPPLYMASTERID  VARCHAR(50) COMMENT '费用申请单ID',
   REPAYTYPE            NUMERIC(38,0) NOT NULL COMMENT '1现金还普通借款
            2现金还备用金借款
            3现金还专项借款
            
            ',
   PROJECTEDREPAYDATE   DATETIME COMMENT '预计还款时间',
   SPECIALBORROWMONEY   NUMERIC(8,0) COMMENT '专项借款金额',
   SIMPLEBORROWMONEY    NUMERIC(8,0) COMMENT '普通借款金额',
   BACKUPBORROWMONEY    NUMERIC(8,0) COMMENT '备用金借款金额',
   TOTALMONEY           NUMERIC(8,0) NOT NULL COMMENT '总金额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   EDITSTATES           NUMERIC(38,0) NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          NUMERIC(38,0) NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   OWNERID              VARCHAR(50) NOT NULL COMMENT '申请人',
   OWNERPOSTID          VARCHAR(50) NOT NULL COMMENT '申请人岗位',
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL COMMENT '申请人部门',
   OWNERCOMPANYID       VARCHAR(50) NOT NULL COMMENT '申请人公司',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建时间',
   CREATECOMPANYID      VARCHAR(50) NOT NULL COMMENT '公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL COMMENT '部门ID',
   CREATEPOSTID         VARCHAR(50) NOT NULL COMMENT '岗位ID',
   UPDATEUSERID         VARCHAR(50) NOT NULL COMMENT '修改人',
   UPDATEDATE           DATETIME NOT NULL COMMENT '修改时间',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人名称',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改人名称',
   OWNERNAME            VARCHAR(50) COMMENT '申请人名称',
   CREATEDEPARTMENTNAME VARCHAR(50) COMMENT '部门名称',
   CREATECOMPANYNAME    VARCHAR(50) COMMENT '公司名称',
   CREATEPOSTNAME       VARCHAR(50) COMMENT '岗位名称',
   OWNERDEPARTMENTNAME  VARCHAR(50) COMMENT '申请人部门名称',
   OWNERCOMPANYNAME     VARCHAR(50) COMMENT '申请人公司名称',
   OWNERPOSTNAME        VARCHAR(50) COMMENT '申请人岗位名称',
   PRIMARY KEY (CHARGEBYREPAYID)
);

ALTER TABLE T_FB_CHARGEBYREPAY COMMENT '冲借款单';

/*==============================================================*/
/* Table: T_FB_COMPANYBUDGETAPPLYDETAIL                         */
/*==============================================================*/
CREATE TABLE T_FB_COMPANYBUDGETAPPLYDETAIL
(
   COMPANYBUDGETAPPLYDETAILID VARCHAR(50) NOT NULL,
   COMPANYBUDGETAPPLYMASTERID VARCHAR(50),
   SUBJECTID            VARCHAR(50),
   BUDGETMONEY          NUMERIC(8,0) NOT NULL,
   AUDITBUDGETMONEY     NUMERIC(8,0),
   DISTRIBUTEDMONDEY    NUMERIC(8,0),
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   LASTBUDGETMONEY      NUMERIC(8,0) COMMENT '上年发生',
   REMARK               VARCHAR(200) COMMENT '备注',
   PRIMARY KEY (COMPANYBUDGETAPPLYDETAILID)
);

ALTER TABLE T_FB_COMPANYBUDGETAPPLYDETAIL COMMENT '公司预算申请明细';

/*==============================================================*/
/* Table: T_FB_COMPANYBUDGETAPPLYMASTER                         */
/*==============================================================*/
CREATE TABLE T_FB_COMPANYBUDGETAPPLYMASTER
(
   COMPANYBUDGETAPPLYMASTERID VARCHAR(50) NOT NULL,
   COMPANYBUDGETAPPLYMASTERCODE VARCHAR(50),
   BUDGETYEAR           INT,
   REMARK               VARCHAR(2000),
   BUDGETMONEY          NUMERIC(8,0),
   AUDITBUDGETMONEY     NUMERIC(8,0),
   DISTRIBUTEDMONDEY    NUMERIC(8,0),
   EDITSTATES           INT NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          INT NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   COMPANYBUDGETSUMMASTERID VARCHAR(50),
   ISVALID              VARCHAR(10),
   PRIMARY KEY (COMPANYBUDGETAPPLYMASTERID)
);

ALTER TABLE T_FB_COMPANYBUDGETAPPLYMASTER COMMENT '年度预算';

/*==============================================================*/
/* Table: T_FB_COMPANYBUDGETMODDETAIL                           */
/*==============================================================*/
CREATE TABLE T_FB_COMPANYBUDGETMODDETAIL
(
   COMPANYBUDGETMODDETAILID VARCHAR(50) NOT NULL,
   COMPANYBUDGETMODMASTERID VARCHAR(50),
   SUBJECTID            VARCHAR(50),
   USABLEMONEY          NUMERIC(8,0),
   AUDITBUDGETMONEY     NUMERIC(8,0),
   BUDGETMONEY          NUMERIC(8,0) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50),
   REMARK               VARCHAR(200) COMMENT '备注',
   PRIMARY KEY (COMPANYBUDGETMODDETAILID)
);

ALTER TABLE T_FB_COMPANYBUDGETMODDETAIL COMMENT '公司预算变更申请单明细';

/*==============================================================*/
/* Table: T_FB_COMPANYBUDGETMODMASTER                           */
/*==============================================================*/
CREATE TABLE T_FB_COMPANYBUDGETMODMASTER
(
   COMPANYBUDGETMODMASTERID VARCHAR(50) NOT NULL,
   COMPANYBUDGETMODMASTERCODE VARCHAR(50),
   BUDGETYEAR           INT,
   REMARK               VARCHAR(2000),
   AUDITBUDGETMONEY     NUMERIC(8,0),
   BUDGETMONEY          NUMERIC(8,0),
   EDITSTATES           INT NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          INT NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   PRIMARY KEY (COMPANYBUDGETMODMASTERID)
);

ALTER TABLE T_FB_COMPANYBUDGETMODMASTER COMMENT '公司预算变更申请单';

/*==============================================================*/
/* Table: T_FB_COMPANYBUDGETSUMDETAIL                           */
/*==============================================================*/
CREATE TABLE T_FB_COMPANYBUDGETSUMDETAIL
(
   COMPANYBUDGETSUMDETAILID VARCHAR(50) NOT NULL,
   COMPANYBUDGETAPPLYMASTERID VARCHAR(50),
   COMPANYBUDGETSUMMASTERID VARCHAR(50),
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CHECKSTATES          NUMERIC(38,0) COMMENT '默认为null,3打回',
   PRIMARY KEY (COMPANYBUDGETSUMDETAILID)
);

ALTER TABLE T_FB_COMPANYBUDGETSUMDETAIL COMMENT '年度预算汇总明细';

/*==============================================================*/
/* Table: T_FB_COMPANYBUDGETSUMMASTER                           */
/*==============================================================*/
CREATE TABLE T_FB_COMPANYBUDGETSUMMASTER
(
   COMPANYBUDGETSUMMASTERID VARCHAR(50) NOT NULL,
   COMPANYBUDGETSUMMASTERCODE VARCHAR(50),
   BUDGETYEAR           NUMERIC(38,0),
   REMARK               VARCHAR(2000),
   BUDGETMONEY          NUMERIC(8,0),
   AUDITBUDGETMONEY     NUMERIC(8,0),
   DISTRIBUTEDMONDEY    NUMERIC(8,0),
   EDITSTATES           NUMERIC(38,0) NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          NUMERIC(38,0) NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   SUMSETTINGSMASTERID  VARCHAR(50) COMMENT '预算汇总设置主表ID',
   PARENTID             VARCHAR(50) COMMENT '父级ID',
   SUMLEVEL             NUMERIC(8,0) DEFAULT 0 COMMENT '汇总级别',
   PRIMARY KEY (COMPANYBUDGETSUMMASTERID)
);

ALTER TABLE T_FB_COMPANYBUDGETSUMMASTER COMMENT '年度预算汇总';

/*==============================================================*/
/* Table: T_FB_COMPANYTRANSFERDETAIL                            */
/*==============================================================*/
CREATE TABLE T_FB_COMPANYTRANSFERDETAIL
(
   COMPANYTRANSFERDETAILID VARCHAR(50) NOT NULL,
   SUBJECTID            VARCHAR(50),
   COMPANYTRANSFERMASTERID VARCHAR(50),
   USABLEMONEY          NUMERIC(8,0),
   TRANSFERMONEY        NUMERIC(8,0) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   PRIMARY KEY (COMPANYTRANSFERDETAILID)
);

ALTER TABLE T_FB_COMPANYTRANSFERDETAIL COMMENT '预算调拨单申请单明细';

/*==============================================================*/
/* Table: T_FB_COMPANYTRANSFERMASTER                            */
/*==============================================================*/
CREATE TABLE T_FB_COMPANYTRANSFERMASTER
(
   COMPANYTRANSFERMASTERID VARCHAR(50) NOT NULL,
   COMPANYTRANSFERMASTERCODE VARCHAR(50),
   BUDGETYEAR           INT,
   EDITSTATES           INT NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          INT NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   REMARK               VARCHAR(2000),
   AUDITTRANSFERMONEY   NUMERIC(8,0),
   TRANSFERMONEY        NUMERIC(8,0),
   TRANSFERFROM         VARCHAR(50),
   TRANSFERTO           VARCHAR(50),
   TRANSFERFROMTYPE     INT COMMENT '1 : 公司 ; 2: 部门 ; 3: 个人',
   TRANSFERTOTYPE       INT COMMENT '1 : 公司 ; 2: 部门 ; 3: 个人',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   TRANSFERFROMNAME     VARCHAR(100),
   TRANSFERTONAME       VARCHAR(100),
   PRIMARY KEY (COMPANYTRANSFERMASTERID)
);

ALTER TABLE T_FB_COMPANYTRANSFERMASTER COMMENT '公司预算调拨单申请单';

/*==============================================================*/
/* Table: T_FB_DEPTBUDGETADDDETAIL                              */
/*==============================================================*/
CREATE TABLE T_FB_DEPTBUDGETADDDETAIL
(
   DEPTBUDGETADDDETAILID VARCHAR(50) NOT NULL,
   DEPTBUDGETADDMASTERID VARCHAR(50),
   SUBJECTID            VARCHAR(50),
   BUDGETMONEY          NUMERIC(8,0) NOT NULL,
   AUDITBUDGETMONEY     NUMERIC(8,0),
   PERSONBUDGETMONEY    NUMERIC(8,0),
   TOTALBUDGETMONEY     NUMERIC(8,0),
   USABLEMONEY          NUMERIC(8,0),
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   REMARK               VARCHAR(200) COMMENT '备注',
   PRIMARY KEY (DEPTBUDGETADDDETAILID)
);

ALTER TABLE T_FB_DEPTBUDGETADDDETAIL COMMENT '部门预算补增明细单';

/*==============================================================*/
/* Table: T_FB_DEPTBUDGETADDMASTER                              */
/*==============================================================*/
CREATE TABLE T_FB_DEPTBUDGETADDMASTER
(
   DEPTBUDGETADDMASTERID VARCHAR(50) NOT NULL,
   DEPTBUDGETADDMASTERCODE VARCHAR(50),
   BUDGETARYMONTH       DATETIME NOT NULL,
   EDITSTATES           INT NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          INT NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   REMARK               VARCHAR(2000),
   AUDITBUDGETMONEY     NUMERIC(8,0),
   BUDGETCHARGE         NUMERIC(8,0),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   PRIMARY KEY (DEPTBUDGETADDMASTERID)
);

ALTER TABLE T_FB_DEPTBUDGETADDMASTER COMMENT '部门预算补增申请单';

/*==============================================================*/
/* Table: T_FB_DEPTBUDGETAPPLYDETAIL                            */
/*==============================================================*/
CREATE TABLE T_FB_DEPTBUDGETAPPLYDETAIL
(
   DEPTBUDGETAPPLYDETAILID VARCHAR(50) NOT NULL,
   DEPTBUDGETAPPLYMASTERID VARCHAR(50),
   SUBJECTID            VARCHAR(50),
   PERSONBUDGETAPPLYMASTERID VARCHAR(50),
   SERIALNUMBER         NUMERIC(38,0),
   USABLEMONEY          NUMERIC(8,0),
   BUDGETMONEY          NUMERIC(8,0) NOT NULL,
   AUDITBUDGETMONEY     NUMERIC(8,0),
   LASTBUDGEMONEY       NUMERIC(8,0),
   LASTACTUALBUDGETMONEY NUMERIC(8,0),
   BEGINNINGBUDGETBALANCE NUMERIC(8,0),
   YEARBUDGETBALANCE    NUMERIC(8,0),
   PERSONBUDGETMONEY    NUMERIC(8,0),
   TOTALBUDGETMONEY     NUMERIC(8,0),
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   REMARK               VARCHAR(200) COMMENT '备注',
   PRIMARY KEY (DEPTBUDGETAPPLYDETAILID)
);

ALTER TABLE T_FB_DEPTBUDGETAPPLYDETAIL COMMENT '部门预算申请明细';

/*==============================================================*/
/* Table: T_FB_DEPTBUDGETAPPLYMASTER                            */
/*==============================================================*/
CREATE TABLE T_FB_DEPTBUDGETAPPLYMASTER
(
   DEPTBUDGETAPPLYMASTERID VARCHAR(50) NOT NULL,
   DEPTBUDGETAPPLYMASTERCODE VARCHAR(50),
   BUDGETARYMONTH       DATETIME NOT NULL,
   APPLIEDTYPE          INT COMMENT '1 : 预算申请 2: 补增申请',
   EDITSTATES           INT NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          INT NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   REMARK               VARCHAR(2000),
   BUDGETMONEY          NUMERIC(8,0),
   AUDITBUDGETMONEY     NUMERIC(8,0),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   DEPTBUDGETSUMMASTERID VARCHAR(50),
   ISVALID              VARCHAR(10),
   PRIMARY KEY (DEPTBUDGETAPPLYMASTERID)
);

ALTER TABLE T_FB_DEPTBUDGETAPPLYMASTER COMMENT '部门预算申请';

/*==============================================================*/
/* Table: T_FB_DEPTBUDGETSUMDETAIL                              */
/*==============================================================*/
CREATE TABLE T_FB_DEPTBUDGETSUMDETAIL
(
   DEPTBUDGETSUMDETAILID VARCHAR(50) NOT NULL,
   DEPTBUDGETSUMMASTERID VARCHAR(50),
   DEPTBUDGETAPPLYMASTERID VARCHAR(50),
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CHECKSTATES          NUMERIC(38,0) COMMENT '默认为null,3打回',
   PRIMARY KEY (DEPTBUDGETSUMDETAILID)
);

ALTER TABLE T_FB_DEPTBUDGETSUMDETAIL COMMENT '部门预算汇总明细';

/*==============================================================*/
/* Table: T_FB_DEPTBUDGETSUMMASTER                              */
/*==============================================================*/
CREATE TABLE T_FB_DEPTBUDGETSUMMASTER
(
   DEPTBUDGETSUMMASTERID VARCHAR(50) NOT NULL,
   DEPTBUDGETSUMMASTERCODE VARCHAR(50),
   BUDGETARYMONTH       DATETIME NOT NULL,
   APPLIEDTYPE          NUMERIC(38,0) COMMENT '1 : 预算申请 2: 补增申请',
   EDITSTATES           NUMERIC(38,0) NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          NUMERIC(38,0) NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   REMARK               VARCHAR(2000),
   BUDGETMONEY          NUMERIC(8,0),
   AUDITBUDGETMONEY     NUMERIC(8,0),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   SUMSETTINGSMASTERID  VARCHAR(50) COMMENT '预算汇总设置主表ID',
   PARENTID             VARCHAR(50) COMMENT '父级ID',
   SUMLEVEL             NUMERIC(8,0) COMMENT '汇总级别',
   PRIMARY KEY (DEPTBUDGETSUMMASTERID)
);

ALTER TABLE T_FB_DEPTBUDGETSUMMASTER COMMENT '部门预算汇总';

/*==============================================================*/
/* Table: T_FB_DEPTTRANSFERDETAIL                               */
/*==============================================================*/
CREATE TABLE T_FB_DEPTTRANSFERDETAIL
(
   DEPTTRANSFERDETAILID VARCHAR(50) NOT NULL,
   SUBJECTID            VARCHAR(50),
   DEPTTRANSFERMASTERID VARCHAR(50),
   USABLEMONEY          NUMERIC(8,0),
   AUDITTRANSFERMONEY   NUMERIC(8,0),
   TRANSFERMONEY        NUMERIC(8,0) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   REMARK               VARCHAR(200) COMMENT '备注',
   PRIMARY KEY (DEPTTRANSFERDETAILID)
);

ALTER TABLE T_FB_DEPTTRANSFERDETAIL COMMENT '部门预算调拨单申请单明细';

/*==============================================================*/
/* Table: T_FB_DEPTTRANSFERMASTER                               */
/*==============================================================*/
CREATE TABLE T_FB_DEPTTRANSFERMASTER
(
   DEPTTRANSFERMASTERID VARCHAR(50) NOT NULL,
   DEPTTRANSFERMASTERCODE VARCHAR(50),
   BUDGETARYMONTH       DATETIME NOT NULL,
   EDITSTATES           INT NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          INT NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   REMARK               VARCHAR(2000),
   AUDITTRANSFERMONEY   NUMERIC(8,0),
   TRANSFERMONEY        NUMERIC(8,0),
   TRANSFERFROM         VARCHAR(50),
   TRANSFERTO           VARCHAR(50),
   TRANSFERFROMTYPE     INT COMMENT '1 : 公司 ; 2: 部门 ; 3: 个人',
   TRANSFERTOTYPE       INT COMMENT '1 : 公司 ; 2: 部门 ; 3: 个人',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   TRANSFERFROMNAME     VARCHAR(50),
   TRANSFERTONAME       VARCHAR(50),
   TRANSFERFROMPOSTID   VARCHAR(50),
   TRANSFERTOPOSTID     VARCHAR(50),
   TRANSFERFROMPOSTNAME VARCHAR(50),
   TRANSFERTOPOSTNAME   VARCHAR(50),
   TRANSFERTOCOMPANYID  VARCHAR(50),
   TRANSFERFROMCOMPANYID VARCHAR(50),
   TRANSFERTOCOMPANYNAME VARCHAR(50),
   TRANSFERFROMCOMPANYNAME VARCHAR(50),
   TRANSFERFROMDEPARTMENTID VARCHAR(50),
   TRANSFERTODEPARTMENTID VARCHAR(50),
   TRANSFERFROMDEPARTMENTNAME VARCHAR(50),
   TRANSFERTODEPARTMENTNAME VARCHAR(50),
   PRIMARY KEY (DEPTTRANSFERMASTERID)
);

ALTER TABLE T_FB_DEPTTRANSFERMASTER COMMENT '部门预算调拨单申请单';

/*==============================================================*/
/* Table: T_FB_EXTENSIONORDERDETAIL                             */
/*==============================================================*/
CREATE TABLE T_FB_EXTENSIONORDERDETAIL
(
   EXTENSIONORDERDETAILID VARCHAR(50) NOT NULL,
   EXTENSIONALORDERID   VARCHAR(50),
   SUBJECTID            VARCHAR(50) NOT NULL,
   CHARGETYPE           INT COMMENT '1：个人， 2：公共',
   USABLEMONEY          NUMERIC(8,0),
   REMARK               VARCHAR(200),
   APPLIEDMONEY         NUMERIC(8,0) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   PRIMARY KEY (EXTENSIONORDERDETAILID)
);

ALTER TABLE T_FB_EXTENSIONORDERDETAIL COMMENT '扩展单明细';

/*==============================================================*/
/* Table: T_FB_EXTENSIONALORDER                                 */
/*==============================================================*/
CREATE TABLE T_FB_EXTENSIONALORDER
(
   EXTENSIONALORDERID   VARCHAR(50) NOT NULL,
   EXTENSIONALTYPEID    VARCHAR(50),
   CHECKSTATES          INT NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   ORDERID              VARCHAR(50),
   ORDERCODE            VARCHAR(50),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   INNERORDERTYPE       VARCHAR(50),
   INNERORDERID         VARCHAR(50),
   RECEIVER             VARCHAR(50),
   BANK                 VARCHAR(50),
   BANKACCOUT           VARCHAR(50),
   PAYTARGET            INT COMMENT '1 : 付本人
            2 : 付其帐号',
   TOTALMONEY           NUMERIC(8,0),
   APPLYTYPE            INT COMMENT '1,费用报销
            2.借款',
   REMARK               VARCHAR(2000),
   INNERORDERCODE       VARCHAR(50) COMMENT '内部单据编号',
   PAYMENTINFO          VARCHAR(2000) COMMENT '支付信息(打印专用)',
   PRIMARY KEY (EXTENSIONALORDERID)
);

ALTER TABLE T_FB_EXTENSIONALORDER COMMENT '扩展单据';

/*==============================================================*/
/* Table: T_FB_EXTENSIONALTYPE                                  */
/*==============================================================*/
CREATE TABLE T_FB_EXTENSIONALTYPE
(
   EXTENSIONALTYPEID    VARCHAR(50) NOT NULL,
   EXTENSIONALTYPECODE  VARCHAR(50),
   EXTENSIONALTYPENAME  VARCHAR(50),
   REMARK               VARCHAR(2000),
   MODELCODE            VARCHAR(50),
   SYSTEMCODE           VARCHAR(50),
   PRIMARY KEY (EXTENSIONALTYPEID)
);

ALTER TABLE T_FB_EXTENSIONALTYPE COMMENT '扩展单据类型';

/*==============================================================*/
/* Table: T_FB_ORDERCODE                                        */
/*==============================================================*/
CREATE TABLE T_FB_ORDERCODE
(
   ORDERCODEID          VARCHAR(50),
   TABLENAME            VARCHAR(50) NOT NULL,
   FIELDNAME            VARCHAR(50) NOT NULL,
   PRENAME              VARCHAR(50),
   CURRENTDATE          DATETIME,
   RUNNINGNUMBER        INT,
   REMARK               VARCHAR(250),
   ORDERCODESTYLE       VARCHAR(100),
   UPDATEDATE           DATETIME,
   UPDATEUSERID         VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME,
   CREATECOMPANYID      VARCHAR(50),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   CREATEUSERNAME       VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   PRIMARY KEY (TABLENAME)
);

ALTER TABLE T_FB_ORDERCODE COMMENT '单据编号生成规则表';

/*==============================================================*/
/* Table: T_FB_PERSONTRANSFERDETAIL                             */
/*==============================================================*/
CREATE TABLE T_FB_PERSONTRANSFERDETAIL
(
   PERSONTRANSFERDETAILID VARCHAR(50) NOT NULL COMMENT '个人预算调拨明细ID',
   DEPTTRANSFERDETAILID VARCHAR(50) COMMENT '预算调拨单申请单明细ID',
   SUBJECTID            VARCHAR(50) COMMENT '科目ID',
   BUDGETMONEY          NUMERIC(8,0) COMMENT '资金预算',
   LIMITBUDGETMONEY     NUMERIC(8,0) COMMENT '最大预算金额',
   USABLEMONEY          NUMERIC(8,0) COMMENT '可用金额',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) NOT NULL COMMENT '修改人',
   UPDATEDATE           DATETIME NOT NULL COMMENT '修改时间',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人名称',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改人名称',
   OWNERNAME            VARCHAR(50) COMMENT '申请人名称',
   OWNERID              VARCHAR(50) NOT NULL COMMENT '申请人',
   OWNERPOSTID          VARCHAR(50) COMMENT '申请人岗位',
   OWNERPOSTNAME        VARCHAR(50) COMMENT '申请人岗位名称',
   REMARK               VARCHAR(200) COMMENT '摘要',
   PRIMARY KEY (PERSONTRANSFERDETAILID)
);

ALTER TABLE T_FB_PERSONTRANSFERDETAIL COMMENT '个人预算调拨明细';

/*==============================================================*/
/* Table: T_FB_PERSONACCOUNT                                    */
/*==============================================================*/
CREATE TABLE T_FB_PERSONACCOUNT
(
   PERSONACCOUNTID      VARCHAR(50) NOT NULL,
   NEXTREPAYDATE        DATETIME COMMENT '
            ',
   REMARK               VARCHAR(2000),
   SPECIALBORROWMONEY   NUMERIC(8,0) COMMENT '现金还专项借款',
   SIMPLEBORROWMONEY    NUMERIC(8,0) COMMENT '现金还普通借款',
   BACKUPBORROWMONEY    NUMERIC(8,0) COMMENT '现金还备用金借款',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME,
   CREATECOMPANYID      VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME,
   BORROWMONEY          NUMERIC(8,0) COMMENT '借款总金额',
   PRIMARY KEY (PERSONACCOUNTID)
);

ALTER TABLE T_FB_PERSONACCOUNT COMMENT '往来明细总账';

/*==============================================================*/
/* Table: T_FB_PERSONBUDGETADDDETAIL                            */
/*==============================================================*/
CREATE TABLE T_FB_PERSONBUDGETADDDETAIL
(
   PERSONBUDGETADDDETAILID VARCHAR(50) NOT NULL,
   SUBJECTID            VARCHAR(50),
   DEPTBUDGETADDDETAILID VARCHAR(50),
   BUDGETMONEY          NUMERIC(8,0),
   LIMITBUDGETMONEY     NUMERIC(8,0),
   USABLEMONEY          NUMERIC(8,0),
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   PRIMARY KEY (PERSONBUDGETADDDETAILID)
);

ALTER TABLE T_FB_PERSONBUDGETADDDETAIL COMMENT '个人预算补增明细';

/*==============================================================*/
/* Table: T_FB_PERSONBUDGETAPPLYDETAIL                          */
/*==============================================================*/
CREATE TABLE T_FB_PERSONBUDGETAPPLYDETAIL
(
   PERSONBUDGETAPPLYDETAILID VARCHAR(50) NOT NULL,
   SUBJECTID            VARCHAR(50),
   DEPTBUDGETAPPLYDETAILID VARCHAR(50),
   BUDGETMONEY          NUMERIC(8,0),
   LIMITBUDGETMONEY     NUMERIC(8,0),
   USABLEMONEY          NUMERIC(8,0),
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   REMARK               VARCHAR(200) COMMENT '备注',
   PRIMARY KEY (PERSONBUDGETAPPLYDETAILID)
);

ALTER TABLE T_FB_PERSONBUDGETAPPLYDETAIL COMMENT '个人预算申请明细';

/*==============================================================*/
/* Table: T_FB_PERSONMONEYASSIGNDETAIL                          */
/*==============================================================*/
CREATE TABLE T_FB_PERSONMONEYASSIGNDETAIL
(
   PERSONBUDGETAPPLYDETAILID VARCHAR(50) NOT NULL,
   PERSONMONEYASSIGNMASTERID VARCHAR(50),
   SUBJECTID            VARCHAR(50),
   BUDGETMONEY          NUMERIC(8,0),
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   REMARK               VARCHAR(200),
   POSTINFO             VARCHAR(200),
   SUGGESTBUDGETMONEY   NUMERIC(8,0) COMMENT '下拨额度参考',
   POSTLEVEL            NUMERIC(8,0),
   PRIMARY KEY (PERSONBUDGETAPPLYDETAILID)
);

ALTER TABLE T_FB_PERSONMONEYASSIGNDETAIL COMMENT '个人经费下拨明细';

/*==============================================================*/
/* Table: T_FB_PERSONMONEYASSIGNMASTER                          */
/*==============================================================*/
CREATE TABLE T_FB_PERSONMONEYASSIGNMASTER
(
   PERSONMONEYASSIGNMASTERID VARCHAR(50) NOT NULL,
   PERSONMONEYASSIGNMASTERCODE VARCHAR(50),
   BUDGETARYMONTH       DATETIME NOT NULL,
   APPLIEDTYPE          NUMERIC(38,0) COMMENT '1 : 预算申请 2: 补增申请',
   EDITSTATES           NUMERIC(38,0) NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          NUMERIC(38,0) NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   REMARK               VARCHAR(2000),
   BUDGETMONEY          NUMERIC(8,0),
   AUDITBUDGETMONEY     NUMERIC(8,0),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   ASSIGNCOMPANYID      VARCHAR(50) COMMENT '下拨公司',
   ASSIGNCOMPANYNAME    VARCHAR(50) COMMENT '下拨公司名称',
   PRIMARY KEY (PERSONMONEYASSIGNMASTERID)
);

ALTER TABLE T_FB_PERSONMONEYASSIGNMASTER COMMENT '个人经费下拨申请单';

/*==============================================================*/
/* Table: T_FB_REPAYAPPLYDETAIL                                 */
/*==============================================================*/
CREATE TABLE T_FB_REPAYAPPLYDETAIL
(
   REPAYAPPLYDETAILID   VARCHAR(50) NOT NULL,
   SUBJECTID            VARCHAR(50),
   REPAYAPPLYMASTERID   VARCHAR(50),
   BORROWAPPLYDETAILID  VARCHAR(50),
   REMARK               VARCHAR(200),
   CHARGETYPE           INT COMMENT '1：个人， 2：公共',
   BORROWMONEY          NUMERIC(8,0) NOT NULL,
   REPAYMONEY           NUMERIC(8,0),
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   REPAYTYPE            NUMERIC(38,0) COMMENT '1现金还普通借款
            2现金还备用金借款
            3现金还专项借款',
   PRIMARY KEY (REPAYAPPLYDETAILID)
);

ALTER TABLE T_FB_REPAYAPPLYDETAIL COMMENT '还款申请明细';

/*==============================================================*/
/* Table: T_FB_REPAYAPPLYMASTER                                 */
/*==============================================================*/
CREATE TABLE T_FB_REPAYAPPLYMASTER
(
   REPAYAPPLYMASTERID   VARCHAR(50) NOT NULL,
   EXTENSIONALORDERID   VARCHAR(50),
   BORROWAPPLYMASTERID  VARCHAR(50),
   REPAYAPPLYCODE       VARCHAR(50),
   REPAYTYPE            INT COMMENT '1现金还普通借款
            2现金还备用金借款
            3现金还专项借款
            
            ',
   PROJECTEDREPAYDATE   DATETIME,
   BRORROWEDMONEY       NUMERIC(8,0),
   TOTALMONEY           NUMERIC(8,0) NOT NULL,
   REMARK               VARCHAR(2000),
   EDITSTATES           INT NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          INT NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   PAYMENTINFO          VARCHAR(2000) COMMENT '支付信息(打印专用)',
   PRIMARY KEY (REPAYAPPLYMASTERID)
);

ALTER TABLE T_FB_REPAYAPPLYMASTER COMMENT '还款申请单';

/*==============================================================*/
/* Table: T_FB_SUBJECTCOMPANYSET                                */
/*==============================================================*/
CREATE TABLE T_FB_SUBJECTCOMPANYSET
(
   SUBJECTCOMPANYID     VARCHAR(50) NOT NULL,
   SUBJECTID            VARCHAR(50),
   ACTIVED              INT NOT NULL COMMENT '1 : 可用 ; 0 : 不可用',
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   PRIMARY KEY (SUBJECTCOMPANYID)
);

ALTER TABLE T_FB_SUBJECTCOMPANYSET COMMENT '公司科目设置';

/*==============================================================*/
/* Table: T_FB_SUMSETTINGSDETAIL                                */
/*==============================================================*/
CREATE TABLE T_FB_SUMSETTINGSDETAIL
(
   SUMSETTINGSDETAILID  VARCHAR(50) NOT NULL,
   SUMSETTINGSMASTERID  VARCHAR(50) NOT NULL,
   EDITSTATES           NUMERIC(38,0) NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人名称',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改人名称',
   OWNERID              VARCHAR(50) COMMENT '申请人',
   OWNERPOSTID          VARCHAR(50) COMMENT '申请人岗位',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '申请人部门',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '申请人公司',
   OWNERNAME            VARCHAR(50) COMMENT '申请人名称',
   OWNERDEPARTMENTNAME  VARCHAR(50) COMMENT '申请人部门名称',
   OWNERCOMPANYNAME     VARCHAR(50) COMMENT '申请人公司名称',
   OWNERPOSTNAME        VARCHAR(50) COMMENT '申请人岗位名称',
   PRIMARY KEY (SUMSETTINGSDETAILID)
);

ALTER TABLE T_FB_SUMSETTINGSDETAIL COMMENT '预算汇总设置明细';

/*==============================================================*/
/* Table: T_FB_SUMSETTINGSMASTER                                */
/*==============================================================*/
CREATE TABLE T_FB_SUMSETTINGSMASTER
(
   SUMSETTINGSMASTERID  VARCHAR(50) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL COMMENT '申请人',
   OWNERPOSTID          VARCHAR(50) NOT NULL COMMENT '申请人岗位',
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL COMMENT '申请人部门',
   OWNERCOMPANYID       VARCHAR(50) NOT NULL COMMENT '申请人公司',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建时间',
   CREATECOMPANYID      VARCHAR(50) NOT NULL COMMENT '公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL COMMENT '部门ID',
   CREATEPOSTID         VARCHAR(50) NOT NULL COMMENT '岗位ID',
   UPDATEUSERID         VARCHAR(50) NOT NULL COMMENT '修改人',
   UPDATEDATE           DATETIME NOT NULL COMMENT '修改时间',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人名称',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改人名称',
   OWNERNAME            VARCHAR(50) COMMENT '申请人名称',
   CREATEDEPARTMENTNAME VARCHAR(50) COMMENT '部门名称',
   CREATECOMPANYNAME    VARCHAR(50) COMMENT '公司名称',
   CREATEPOSTNAME       VARCHAR(50) COMMENT '岗位名称',
   OWNERDEPARTMENTNAME  VARCHAR(50) COMMENT '申请人部门名称',
   OWNERCOMPANYNAME     VARCHAR(50) COMMENT '申请人公司名称',
   OWNERPOSTNAME        VARCHAR(50) COMMENT '申请人岗位名称',
   REMARK               VARCHAR(2000),
   CHECKSTATES          NUMERIC(38,0) COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   EDITSTATES           NUMERIC(38,0) COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   PRIMARY KEY (SUMSETTINGSMASTERID)
);

ALTER TABLE T_FB_SUMSETTINGSMASTER COMMENT '预算汇总设置主表';

/*==============================================================*/
/* Table: T_FB_SALARYPAYLIST                                    */
/*==============================================================*/
CREATE TABLE T_FB_SALARYPAYLIST
(
   SALARYPAYLISTID      VARCHAR(50) NOT NULL,
   PAYYEAR              NUMERIC(38,0),
   PAYMONTH             NUMERIC(38,0),
   PAYMONEY             NUMERIC(8,0),
   EDITSTATES           NUMERIC(38,0) COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          NUMERIC(38,0) COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME,
   CREATECOMPANYID      VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME,
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   PRIMARY KEY (SALARYPAYLISTID)
);

ALTER TABLE T_FB_SALARYPAYLIST COMMENT '工资发放列记录表';

/*==============================================================*/
/* Table: T_FB_SUBJECT                                          */
/*==============================================================*/
CREATE TABLE T_FB_SUBJECT
(
   SUBJECTID            VARCHAR(50) NOT NULL,
   SUBJECTTYPEID        VARCHAR(50),
   PARENTSUBJECTID      VARCHAR(50),
   SUBJECTNAME          VARCHAR(50) NOT NULL,
   SUBJECTCODE          VARCHAR(50) NOT NULL,
   ACTIVED              INT NOT NULL COMMENT '1 : 可用 ; 0 : 不可用',
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   EDITSTATES           INT NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   OWNERNAME            VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   CHECKSTATES          INT NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved
            ',
   PRIMARY KEY (SUBJECTID)
);

ALTER TABLE T_FB_SUBJECT COMMENT '科目';

/*==============================================================*/
/* Table: T_FB_SUBJECTCOMPANY                                   */
/*==============================================================*/
CREATE TABLE T_FB_SUBJECTCOMPANY
(
   SUBJECTCOMPANYID     VARCHAR(50) NOT NULL,
   SUBJECTID            VARCHAR(50),
   ACTIVED              INT NOT NULL COMMENT '1 : 可用 ; 0 : 不可用',
   ISMONTHADJUST        INT COMMENT '科目可以进行调拨。即在月度预算调拨可以调拨这个科目
            0: 不可以 1: 可以',
   ISYEARBUDGET         INT COMMENT '表示月度预算受年度预算限制
            1 : 限制 0: 不限制',
   CONTROLTYPE          INT COMMENT '1 : 不能跨年使用; 2 : 不能跨月使用 ; 3: 无限制 ; 4: 殊年结',
   ISMONTHLIMIT         INT COMMENT '表示在报销时不能超过实时额度
             0 : 不控制 ; 1:控制',
   ISPERSON             INT COMMENT '表示是否可以用于个人费用
            0:  不可以 ;  1:  可以',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   EDITSTATES           INT NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          NUMERIC(38,0),
   PRIMARY KEY (SUBJECTCOMPANYID)
);

ALTER TABLE T_FB_SUBJECTCOMPANY COMMENT '公司科目维护表';

/*==============================================================*/
/* Table: T_FB_SUBJECTDEPTMENT                                  */
/*==============================================================*/
CREATE TABLE T_FB_SUBJECTDEPTMENT
(
   SUBJECTDEPTMENTID    VARCHAR(50) NOT NULL,
   SUBJECTCOMPANYID     VARCHAR(50),
   SUBJECTID            VARCHAR(50),
   ACTIVED              INT NOT NULL COMMENT '1 : 可用 ; 0 : 不可用',
   LIMITBUDGEMONEY      NUMERIC(8,0),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   EDITSTATES           INT NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          NUMERIC(38,0),
   ISPERSON             NUMERIC(38,0),
   PRIMARY KEY (SUBJECTDEPTMENTID)
);

ALTER TABLE T_FB_SUBJECTDEPTMENT COMMENT '部门科目维护表';

/*==============================================================*/
/* Table: T_FB_SUBJECTPOST                                      */
/*==============================================================*/
CREATE TABLE T_FB_SUBJECTPOST
(
   SUBJECTPOSTID        VARCHAR(50) NOT NULL,
   SUBJECTDEPTMENTID    VARCHAR(50),
   SUBJECTID            VARCHAR(50),
   ACTIVED              INT NOT NULL COMMENT '1 : 可用 ; 0 : 不可用',
   LIMITBUDGEMONEY      NUMERIC(8,0),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   EDITSTATES           INT NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          NUMERIC(38,0),
   ISPERSON             NUMERIC(38,0) COMMENT '表示是否可以用于个人费用
            0:  不可以 ;  1:  可以',
   PRIMARY KEY (SUBJECTPOSTID)
);

ALTER TABLE T_FB_SUBJECTPOST COMMENT '岗位科目维护表';

/*==============================================================*/
/* Table: T_FB_SUBJECTTYPE                                      */
/*==============================================================*/
CREATE TABLE T_FB_SUBJECTTYPE
(
   SUBJECTTYPEID        VARCHAR(50) NOT NULL,
   SUBJECTTYPECODE      VARCHAR(50),
   SUBJECTTYPENAME      VARCHAR(50),
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   EDITSTATES           INT NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   PRIMARY KEY (SUBJECTTYPEID)
);

ALTER TABLE T_FB_SUBJECTTYPE COMMENT '科目类别';

/*==============================================================*/
/* Table: T_FB_SYSTEMSETTINGS                                   */
/*==============================================================*/
CREATE TABLE T_FB_SYSTEMSETTINGS
(
   SYSTEMSETTINGSID     VARCHAR(50) NOT NULL,
   SALARYSUBJECTID      VARCHAR(50),
   TRANVERLSUBJECTID    VARCHAR(50),
   UPDATEDATE           DATETIME,
   UPDATEUSERID         VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME,
   CREATECOMPANYID      VARCHAR(50),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   CREATEUSERNAME       VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   CHECKDATE            DATETIME COMMENT '每月的第几天，几点。',
   ENTERTAINMENTLSUBJECTID VARCHAR(50),
   MONEYASSIGNSUBJECTID VARCHAR(50),
   LASTCHECKDATE        DATETIME,
   PRIMARY KEY (SYSTEMSETTINGSID)
);

ALTER TABLE T_FB_SYSTEMSETTINGS COMMENT '系统参数设置';

/*==============================================================*/
/* Table: T_FB_TEMP                                             */
/*==============================================================*/
CREATE TABLE T_FB_TEMP
(
   SUBJECTCODE          VARCHAR(50) NOT NULL,
   WORKPLANTYPE         VARCHAR(50),
   WORKPLANASIGNTYPE    VARCHAR(50),
   LEVEL1               VARCHAR(50),
   LEVEL2               VARCHAR(50),
   LEVEL3               VARCHAR(50),
   LEVEL4               VARCHAR(50),
   PERANTID             VARCHAR(50)
);

ALTER TABLE T_FB_TEMP COMMENT 'T_FB_TEMP';

/*==============================================================*/
/* Table: T_FB_TRAVELEXPAPPLYDETAIL                             */
/*==============================================================*/
CREATE TABLE T_FB_TRAVELEXPAPPLYDETAIL
(
   TRAVELEXPAPPLYDETAILID VARCHAR(50) NOT NULL,
   TRAVELEXPAPPLYMASTERID VARCHAR(50),
   SERIALNUMBER         INT,
   SUBJECTID            VARCHAR(50) NOT NULL,
   BORROWAPPLYDETAILID  VARCHAR(50),
   MONTH                INT NOT NULL,
   DAY                  INT NOT NULL,
   CHARGETYPE           INT COMMENT '1：个人， 2：公共',
   USABLEMONEY          NUMERIC(8,0),
   REPAYMONEY           NUMERIC(8,0),
   REMARK               VARCHAR(200),
   TOTALDAY             INT,
   AIRFARE              NUMERIC(8,0),
   CARFARE              NUMERIC(8,0),
   LODGINGEXPENSES      NUMERIC(8,0),
   TRAVELLINGALLOWANCE  NUMERIC(8,0),
   LODGESAVINGEXPENSES  NUMERIC(8,0),
   OTHERCHARGE          NUMERIC(8,0),
   TOTALCHARGE          NUMERIC(8,0) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   AUDITCHARGEMONEY     NUMERIC(8,0),
   PRIMARY KEY (TRAVELEXPAPPLYDETAILID)
);

ALTER TABLE T_FB_TRAVELEXPAPPLYDETAIL COMMENT '差旅报销申请单明细';

/*==============================================================*/
/* Table: T_FB_TRAVELEXPAPPLYMASTER                             */
/*==============================================================*/
CREATE TABLE T_FB_TRAVELEXPAPPLYMASTER
(
   TRAVELEXPAPPLYMASTERID VARCHAR(50) NOT NULL,
   EXTENSIONALORDERID   VARCHAR(50),
   BORROWAPPLYMASTERID  VARCHAR(50),
   TRAVELEXPAPPLYMASTERCODE VARCHAR(50) NOT NULL,
   BUDGETARYMONTH       DATETIME NOT NULL,
   PAYTYPE              INT NOT NULL COMMENT '1个人费用报销、2冲借款、3冲预付款、4付客户款、5其他',
   REMARK               VARCHAR(2000),
   TOTALMONEY           NUMERIC(8,0) NOT NULL,
   REPAYMENT            NUMERIC(8,0),
   EDITSTATES           INT NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled
            ',
   CHECKSTATES          INT NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   UPDATEUSERID         VARCHAR(50) NOT NULL,
   UPDATEDATE           DATETIME NOT NULL,
   CREATEUSERNAME       VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   CREATEDEPARTMENTNAME VARCHAR(50),
   CREATECOMPANYNAME    VARCHAR(50),
   CREATEPOSTNAME       VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   AUDITCHARGEMONEY     NUMERIC(8,0),
   PRIMARY KEY (TRAVELEXPAPPLYMASTERID)
);

ALTER TABLE T_FB_TRAVELEXPAPPLYMASTER COMMENT '差旅报销申请单';

/*==============================================================*/
/* Table: T_FB_WFBUDGETACCOUNT                                  */
/*==============================================================*/
CREATE TABLE T_FB_WFBUDGETACCOUNT
(
   WFBUDGETACCOUNTID    VARCHAR(50) NOT NULL COMMENT '流水帐ID',
   BUDGETACCOUNTID      VARCHAR(50) COMMENT '预算总账ID',
   ACCOUNTOBJECTTYPE    NUMERIC(38,0) COMMENT '1 : 公司, 2 : 部门, 3 : 个人',
   BUDGETYEAR           NUMERIC(38,0) COMMENT '当前年份',
   BUDGETMONTH          NUMERIC(38,0) COMMENT '当前月份',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '部门ID',
   OWNERID              VARCHAR(50) COMMENT '职员ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '岗位ID',
   SUBJECTID            VARCHAR(50) COMMENT '科目ID',
   BUDGETMONEY          NUMERIC(8,0) COMMENT '预算额度',
   USABLEMONEY          NUMERIC(8,0) COMMENT '可用额度',
   ACTUALMONEY          NUMERIC(8,0) COMMENT '实际额度',
   PAIEDMONEY           NUMERIC(8,0) COMMENT '已报销金额',
   ORDERDETAILID        VARCHAR(50) COMMENT '单据明细ID',
   ORDERTYPE            VARCHAR(50) COMMENT '单据类型： 年/月度预算/增补等',
   ORDERID              VARCHAR(50) COMMENT '明细记录的ID？',
   ORDERCODE            VARCHAR(50) COMMENT '操作单据编号',
   OPERATIONMONEY       NUMERIC(8,0) COMMENT '操作数值',
   TRIGGERBY            VARCHAR(50) COMMENT '提交人或审核人',
   TRIGGEREVENT         VARCHAR(50) COMMENT '提交、审核不通过、审核通过',
   REMARK               VARCHAR(200) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (WFBUDGETACCOUNTID)
);

ALTER TABLE T_FB_WFBUDGETACCOUNT COMMENT '流水帐_预算总帐';

/*==============================================================*/
/* Table: T_FB_WFPERSONACCOUNT                                  */
/*==============================================================*/
CREATE TABLE T_FB_WFPERSONACCOUNT
(
   WFPERSONACCOUNTID    VARCHAR(50) NOT NULL COMMENT '流水帐ID',
   PERSONACCOUNTID      VARCHAR(50) COMMENT '往来明细总账ID',
   BORROWMONEY          NUMERIC(8,0) COMMENT '借款总金额',
   NEXTREPAYDATE        DATETIME COMMENT '
            ',
   SPECIALBORROWMONEY   NUMERIC(8,0) COMMENT '专项借款金额',
   SIMPLEBORROWMONEY    NUMERIC(8,0) COMMENT '普通借款金额',
   BACKUPBORROWMONEY    NUMERIC(8,0) COMMENT '备用金借款金额',
   OWNERID              VARCHAR(50) COMMENT '申请人',
   OWNERPOSTID          VARCHAR(50) COMMENT '申请人岗位',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '申请人部门',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '申请人公司',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATECOMPANYID      VARCHAR(50) COMMENT '公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '部门ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '岗位ID',
   ORDERDETAILID        VARCHAR(50) COMMENT '单据明细ID',
   ORDERTYPE            VARCHAR(50) COMMENT '单据类型：借款、还款、冲借款',
   ORDERID              VARCHAR(50) COMMENT '明细记录的ID？',
   ORDERCODE            VARCHAR(50) COMMENT '操作单据编号',
   OPERATIONMONEY       NUMERIC(8,0) COMMENT '操作数值',
   TRIGGERBY            VARCHAR(50) COMMENT '提交人或审核人',
   TRIGGEREVENT         VARCHAR(50) COMMENT '提交、审核不通过、审核通过',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (WFPERSONACCOUNTID)
);

ALTER TABLE T_FB_WFPERSONACCOUNT COMMENT '流水帐_往来明细总账';

/*==============================================================*/
/* Table: T_FB_WFSUBJECTSETTING                                 */
/*==============================================================*/
CREATE TABLE T_FB_WFSUBJECTSETTING
(
   WFSUBJECTSETTINGID   VARCHAR(50) NOT NULL COMMENT '流水帐ID',
   SUBJECTID            VARCHAR(50) COMMENT '科目ID',
   ACTIVED              NUMERIC(38,0) COMMENT '1 : 可用 ; 0 : 不可用',
   ISMONTHADJUST        NUMERIC(38,0) COMMENT '科目可以进行调拨。即在月度预算调拨可以调拨这个科目
            0: 不可以 1: 可以',
   ISYEARBUDGET         NUMERIC(38,0) COMMENT '表示月度预算受年度预算限制
            0 : 限制 1: 不限制',
   CONTROLTYPE          NUMERIC(38,0) COMMENT '1 : 不能跨月使用; 2 : 不那跨年使用 ; 3: 无限制 ; 4: 殊年结',
   ISMONTHLIMIT         NUMERIC(38,0) COMMENT '表示在报销时不能超过实时额度
             0 : 不控制 ; 1:控制',
   ISPERSON             NUMERIC(38,0) COMMENT '表示是否可以用于个人费用
            0:  不可以 ;  1:  可以',
   LIMITBUDGEMONEY      NUMERIC(8,0) COMMENT '最大预算额度',
   OWNERPOSTID          VARCHAR(50) COMMENT '科目所属岗位',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '科目所属部门',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '科目所属公司',
   OWNERPOSTNAME        VARCHAR(50) COMMENT '科目所属岗位名称',
   OWNERDEPARTMENTNAME  VARCHAR(50) COMMENT '科目所属部门名称',
   OWNERCOMPANYNAME     VARCHAR(50) COMMENT '科目所属公司名称',
   ORDERTYPE            VARCHAR(50) COMMENT '单据类型： 1:公司科目设置, 2:部门科目设置,3:岗位科目设置',
   ORDERID              VARCHAR(50) COMMENT '相关的设置表主键ID',
   TRIGGEREVENT         VARCHAR(50) COMMENT '提交、审核不通过、审核通过、直接保存',
   REMARK               VARCHAR(200) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (WFSUBJECTSETTINGID)
);

ALTER TABLE T_FB_WFSUBJECTSETTING COMMENT '流水帐_科目设置';

/*==============================================================*/
/* Table: T_HR_ACCESSRECORD                                     */
/*==============================================================*/
CREATE TABLE T_HR_ACCESSRECORD
(
   ACCESSRECORD         VARCHAR(50) NOT NULL,
   EMPLOYEENAME         VARCHAR(2000),
   EMPLOYEEID           VARCHAR(2000),
   EMPLOYEECODE         VARCHAR(50),
   CARDTIME             DATETIME,
   MACHINEID            VARCHAR(50),
   MACHINEAREA          VARCHAR(2000),
   CARDNUMBER           VARCHAR(50),
   OUTSTATE             VARCHAR(50),
   CHECKSTATE           VARCHAR(50),
   REMARK               VARCHAR(2000),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME ,
   OWNERCOMPANYID       VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERID              VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   PRIMARY KEY (ACCESSRECORD)
);

ALTER TABLE T_HR_ACCESSRECORD COMMENT '门禁记录表';

/*==============================================================*/
/* Table: T_HR_ADJUSTLEAVE                                      */
/*==============================================================*/
CREATE TABLE T_HR_ADJUSTLEAVE
(
   ADJUSTLEAVEID        VARCHAR(50) NOT NULL COMMENT '请假调休ID',
   LEAVERECORDID        VARCHAR(50) COMMENT '请假记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   LEAVETYPESETID       VARCHAR(50) COMMENT '请假类型ID',
   OFFSEOBJECTTYPE      VARCHAR(50) COMMENT '0 加班记录, 1 带薪假福利',
   OFFSETDAYS           NUMERIC(8,0) COMMENT '调休时长',
   BEGINTIME            DATETIME COMMENT '开始时间',
   ENDTIME              DATETIME COMMENT '结束时间',
   ADJUSTLEAVEDAYS      NUMERIC(8,0) COMMENT '调休天数',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   PRIMARY KEY (ADJUSTLEAVEID)
);

ALTER TABLE T_HR_ADJUSTLEAVE COMMENT '记录员工请假调休的情况
调休可从加班中调休
也可从调休福利中扣除';

/*==============================================================*/
/* Table: T_HR_AREAALLOWANCE                                    */
/*==============================================================*/
CREATE TABLE T_HR_AREAALLOWANCE
(
   AREAALLOWANCEID      VARCHAR(50) NOT NULL COMMENT '地区补贴ID',
   AREADIFFERENCEID     VARCHAR(50) COMMENT '地区分类ID',
   POSTLEVEL            VARCHAR(50) COMMENT '岗位等级',
   ALLOWANCE            NUMERIC(8,0) COMMENT '补贴',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (AREAALLOWANCEID)
);

ALTER TABLE T_HR_AREAALLOWANCE COMMENT '地区差异补贴';

/*==============================================================*/
/* Table: T_HR_AREACITY                                         */
/*==============================================================*/
CREATE TABLE T_HR_AREACITY
(
   AREACITYID           VARCHAR(50) NOT NULL COMMENT '地区分类城市ID',
   AREADIFFERENCEID     VARCHAR(50) COMMENT '地区分类ID',
   CITY                 VARCHAR(10) COMMENT '所在地城市，系统字典中定义',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (AREACITYID)
);

ALTER TABLE T_HR_AREACITY COMMENT '地区分类城市';

/*==============================================================*/
/* Table: T_HR_AREADIFFERENCE                                   */
/*==============================================================*/
CREATE TABLE T_HR_AREADIFFERENCE
(
   AREADIFFERENCEID     VARCHAR(50) NOT NULL COMMENT '地区分类ID',
   AREACATEGORY         VARCHAR(50) COMMENT '地区分类名',
   AREAINDEX            NUMERIC(8,0) COMMENT '地区分类序号',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   PRIMARY KEY (AREADIFFERENCEID)
);

ALTER TABLE T_HR_AREADIFFERENCE COMMENT '地区差异分类表';

/*==============================================================*/
/* Table: T_HR_ASSESSMENTFORMDETAIL                             */
/*==============================================================*/
CREATE TABLE T_HR_ASSESSMENTFORMDETAIL
(
   ASSESSMENTFORMDETAILID VARCHAR(50) NOT NULL COMMENT '考核明细表ID',
   ASSESSMENTFORMMASTERID VARCHAR(50) COMMENT '考核主表ID',
   CHECKPOINT           VARCHAR(50) COMMENT '考核项目点',
   CHECKPOINTSETID      VARCHAR(50) COMMENT '考核项目点ID',
   FIRSTNIBSGRADE       VARCHAR(50) COMMENT '上司评分1等级',
   SECONDNIBSGRADE      VARCHAR(50) COMMENT '上司评分2等级',
   FIRSTSCORE           NUMERIC(8,0) COMMENT '上司评分1分数',
   SECONDSCORE          NUMERIC(8,0) COMMENT '上司评分2分数',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (ASSESSMENTFORMDETAILID)
);

ALTER TABLE T_HR_ASSESSMENTFORMDETAIL COMMENT '人事考核明细表';

/*==============================================================*/
/* Table: T_HR_ASSESSMENTFORMMASTER                             */
/*==============================================================*/
CREATE TABLE T_HR_ASSESSMENTFORMMASTER
(
   ASSESSMENTFORMMASTERID VARCHAR(50) NOT NULL COMMENT '考核主表ID',
   BEREGULARID          VARCHAR(50) COMMENT '转正表ID',
   POSTCHANGEID         VARCHAR(50) COMMENT '异动记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEELEVEL        VARCHAR(50) COMMENT '0  普通员工
            1 干部',
   CHECKTYPE            VARCHAR(50) COMMENT '0 转正
            1 异动',
   CHECKPERSON          VARCHAR(50) COMMENT '考核者',
   CHECKSTARTDATE       VARCHAR(50) COMMENT '考核开始日期',
   CHECKENDDATE         VARCHAR(50) COMMENT '考核结束日期',
   CHECKREASON          VARCHAR(50) COMMENT '考核原因',
   FIRSTNIBSGRADESUM    NUMERIC(8,0) COMMENT '上司评分1合计',
   SECONDNIBSGRADESUM   NUMERIC(8,0) COMMENT '上司评分2合计',
   AWARDSSCORE          NUMERIC(8,0) COMMENT '奖励加分',
   PUNISHMENTSCORE      NUMERIC(8,0) COMMENT '处罚减分',
   TOTALSCORE           NUMERIC(8,0) COMMENT '评分合计',
   FIRSTCOMMENT         VARCHAR(2000) COMMENT '一级考核人评语',
   SECONDCOMMENT        VARCHAR(2000) COMMENT '二级考核人评语',
   HRDEPARTMENTCOMMENT  VARCHAR(2000) COMMENT '人力部审核',
   FIRSTCOMMENTNAME     VARCHAR(2000) COMMENT '一级考核人姓名',
   SECONDCOMMENTNAME    VARCHAR(2000) COMMENT '二级考核人姓名',
   HRCOMMENTNAME        VARCHAR(2000) COMMENT '人力部审核人姓名',
   FIRSTCOMMENTDATE     DATETIME COMMENT '一级考核日期',
   SECONDCOMMENTDATE    DATETIME COMMENT '二级考核日期',
   HRCOMMENTDATE        DATETIME COMMENT '人力部审核日期',
   ATTACHMENTPATH       VARCHAR(50) COMMENT '附件路径',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (ASSESSMENTFORMMASTERID)
);

ALTER TABLE T_HR_ASSESSMENTFORMMASTER COMMENT '人事考核主表#';

/*==============================================================*/
/* Table: T_HR_ATTENDFREELEAVE                                  */
/*==============================================================*/
CREATE TABLE T_HR_ATTENDFREELEAVE
(
   ATTENDFREELEAVEID    VARCHAR(50) NOT NULL COMMENT '考勤方案带薪假ID',
   ATTENDANCESOLUTIONID VARCHAR(50) COMMENT '考勤方案ID',
   LEAVETYPESETID       VARCHAR(50) COMMENT '请假类型ID',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (ATTENDFREELEAVEID)
);

ALTER TABLE T_HR_ATTENDFREELEAVE COMMENT '考勤方案带薪假';

/*==============================================================*/
/* Table: T_HR_ATTENDMACHINESET                                 */
/*==============================================================*/
CREATE TABLE T_HR_ATTENDMACHINESET
(
   ATTENDMACHINESETID   VARCHAR(50) NOT NULL COMMENT '考勤机设置ID',
   COMPANYID            VARCHAR(50) COMMENT '所属公司',
   AREA                 VARCHAR(2000) COMMENT '所处位置',
   ATTENDMACHINENAME    VARCHAR(50) COMMENT '考勤机名称',
   ATTENDMACHINECODE    VARCHAR(50) COMMENT '考勤机编号',
   IPADDRESS            VARCHAR(50) COMMENT 'IP地址',
   COMNUMBER            VARCHAR(50) COMMENT '网络端口',
   READDATATYPE         VARCHAR(1) COMMENT '读取数据方式:
            0,网络传输
            1,数据库
            2,文件',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (ATTENDMACHINESETID)
);

ALTER TABLE T_HR_ATTENDMACHINESET COMMENT '考勤机设置';

/*==============================================================*/
/* Table: T_HR_ATTENDMONTHLYBALANCE                             */
/*==============================================================*/
CREATE TABLE T_HR_ATTENDMONTHLYBALANCE
(
   MONTHLYBALANCEID     VARCHAR(50) NOT NULL COMMENT '考勤月度结算ID',
   MONTHLYBATCHID       VARCHAR(50) COMMENT '月度批量结算ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   BALANCEYEAR          NUMERIC(8,0) COMMENT '结算年份',
   BALANCEMONTH         NUMERIC(8,0) COMMENT '结算月份',
   BALANCEDATE          DATETIME COMMENT '结算日期',
   NEEDATTENDDAYS       NUMERIC(8,0) COMMENT '应出勤天数',
   REALATTENDDAYS       NUMERIC(8,0) COMMENT '实际出勤天数',
   LATEDAYS             NUMERIC(8,0) COMMENT '迟到天数',
   LEAVEEARLYDAYS       NUMERIC(8,0) COMMENT '早退天数',
   ABSENTDAYS           NUMERIC(8,0) COMMENT '旷工天数',
   AFFAIRLEAVEDAYS      NUMERIC(8,0) COMMENT '事假天数',
   SICKLEAVEDAYS        NUMERIC(8,0) COMMENT '病假天数',
   OTHERLEAVEDAYS       NUMERIC(8,0) COMMENT '其他假期天数',
   OVERTIMETIMES        NUMERIC(8,0) COMMENT '加班次数',
   OVERTIMESUMHOURS     NUMERIC(8,0) COMMENT '加班累计时间',
   OVERTIMESUMDAYS      NUMERIC(8,0) COMMENT '加班累计天数',
   LATETIMES            NUMERIC(8,0) COMMENT '迟到次数',
   LEAVEEARLYTIMES      NUMERIC(8,0) COMMENT '早退次数',
   FORGETCARDTIMES      NUMERIC(8,0) COMMENT '漏打卡次数',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   REMARK               VARCHAR(2000) COMMENT '备注',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   EVECTIONTIME         NUMERIC(8,0) COMMENT '出差时长',
   ANNUALLEVELDAYS      NUMERIC(8,0) COMMENT '年休假天数',
   LEAVEUSEDDAYS        NUMERIC(8,0) COMMENT '调休假天数',
   MARRYDAYS            NUMERIC(8,0) COMMENT '婚假天数',
   MATERNITYLEAVEDAYS   NUMERIC(8,0) COMMENT '产假天数',
   NURSESDAYS           NUMERIC(8,0) COMMENT '看护假天数',
   FUNERALLEAVEDAYS     NUMERIC(8,0) COMMENT '丧假天数',
   TRIPDAYS             NUMERIC(8,0) COMMENT '路程假天数',
   INJURYLEAVEDAYS      NUMERIC(8,0) COMMENT '工伤假天数',
   WORKSERVICEMONTHS    NUMERIC(8,0) COMMENT '工作服务总月份数（在职年限以月为单位）',
   WORKTIMEPERDAY       NUMERIC(8,0) COMMENT '每天工作时长（小时）',
   LATEMINUTES          NUMERIC(8,0) COMMENT '迟到时长（分钟）',
   ABSENTMINUTES        NUMERIC(8,0) COMMENT '旷工时长（分钟）',
   PRENATALCARELEAVEDAYS NUMERIC(8,0) COMMENT '产前检查假天数',
   REALNEEDATTENDDAYS   NUMERIC(8,0) COMMENT '应出勤天数（实际）',
   OUTAPPLYTIME         NUMERIC(8,0),
   PRIMARY KEY (MONTHLYBALANCEID)
);

ALTER TABLE T_HR_ATTENDMONTHLYBALANCE COMMENT '考勤月度结算表';

/*==============================================================*/
/* Index: IDX_EMPDIYEARMON                                      */
/*==============================================================*/
CREATE INDEX IDX_EMPDIYEARMON ON T_HR_ATTENDMONTHLYBALANCE
(
   EMPLOYEEID,
   BALANCEMONTH,
   BALANCEYEAR
);

/*==============================================================*/
/* Table: T_HR_ATTENDMONTHLYBATCHBALANCE                        */
/*==============================================================*/
CREATE TABLE T_HR_ATTENDMONTHLYBATCHBALANCE
(
   MONTHLYBATCHID       VARCHAR(50) NOT NULL COMMENT '月度批量结算ID',
   BALANCEYEAR          NUMERIC(8,0) COMMENT '结算年份',
   BALANCEMONTH         NUMERIC(8,0) COMMENT '结算月份',
   BALANCEDATE          DATETIME COMMENT '结算日期',
   BALANCEOBJECTTYPE    VARCHAR(1) COMMENT '结算对象类型',
   BALANCEOBJECTID      VARCHAR(50) COMMENT '结算对象Id',
   BALANCEOBJECTNAME    VARCHAR(500) COMMENT '结算对象名',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   REMARK               VARCHAR(2000) COMMENT '备注',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (MONTHLYBATCHID)
);

ALTER TABLE T_HR_ATTENDMONTHLYBATCHBALANCE COMMENT '考勤月度批量结算表';

/*==============================================================*/
/* Table: T_HR_ATTENDMONTHLYLEAVE                               */
/*==============================================================*/
CREATE TABLE T_HR_ATTENDMONTHLYLEAVE
(
   MONTHLYBALANCEID     VARCHAR(50) NOT NULL COMMENT '考勤月度结算ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   REMARK               VARCHAR(2000) COMMENT '备注',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (MONTHLYBALANCEID)
);

ALTER TABLE T_HR_ATTENDMONTHLYLEAVE COMMENT '考勤月结请假明细';

/*==============================================================*/
/* Table: T_HR_ATTENDYEARLYBALANCE                              */
/*==============================================================*/
CREATE TABLE T_HR_ATTENDYEARLYBALANCE
(
   YEARLYBALANCEID      VARCHAR(50) NOT NULL COMMENT '年度结算ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   BALANCEYEAR          NUMERIC(8,0) COMMENT '结算年份',
   BALANCEDATE          DATETIME COMMENT '结算日期',
   LASTANNUALLEVELUNUSEDDAYS NUMERIC(8,0) COMMENT '上年未休的年假天数',
   ANNUALLEAVEUSEDDAYS  NUMERIC(8,0) COMMENT '本年已冲减年假天数',
   ANNUALLEAVESUMDAYS   NUMERIC(8,0) COMMENT '本年年假天数',
   ANNUALLEAVEVALIDDAYS NUMERIC(8,0) COMMENT '当前可用年假天数',
   LASTLEAVEVALIDDAYS   NUMERIC(8,0) COMMENT '上年可用调休天数',
   LEAVEUSEDDAYS        NUMERIC(8,0) COMMENT '本年已冲减调休天数',
   LEAVEVALIDDAYS       NUMERIC(8,0) COMMENT '当前可用调休天数',
   LEAVESUMDAYS         NUMERIC(8,0) COMMENT '本年可用调休天数',
   NEEDATTENDDAYS       NUMERIC(8,0) COMMENT '应出勤天数',
   REALATTENDDAYS       NUMERIC(8,0) COMMENT '实际出勤天数',
   LATEDAYS             NUMERIC(8,0) COMMENT '迟到天数',
   LEAVEEARLYDAYS       NUMERIC(8,0) COMMENT '早退天数',
   ABSENTDAYS           NUMERIC(8,0) COMMENT '旷工天数',
   AFFAIRLEAVEDAYS      NUMERIC(8,0) COMMENT '事假天数',
   SICKLEAVEDAYS        NUMERIC(8,0) COMMENT '病假天数',
   OTHERLEAVEDAYS       NUMERIC(8,0) COMMENT '其他假期天数',
   OVERTIMETIMES        NUMERIC(8,0) COMMENT '加班次数',
   OVERTIMESUMHOURS     NUMERIC(8,0) COMMENT '加班累计时间',
   OVERTIMESUMDAYS      NUMERIC(8,0) COMMENT '加班累计天数',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   REMARK               VARCHAR(2000) COMMENT '备注',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   EVECTIONTIME         NUMERIC(8,0) COMMENT '出差时长',
   PRIMARY KEY (YEARLYBALANCEID)
);

ALTER TABLE T_HR_ATTENDYEARLYBALANCE COMMENT '考勤年度结算表';

/*==============================================================*/
/* Table: T_HR_ATTENDANCEDEDUCTDETAIL                           */
/*==============================================================*/
CREATE TABLE T_HR_ATTENDANCEDEDUCTDETAIL
(
   DEDUCTDETAILID       VARCHAR(50) NOT NULL COMMENT '考勤异常扣款明细ID',
   DEDUCTMASTERID       VARCHAR(50) COMMENT '考勤异常扣款主表ID',
   FINETYPE             VARCHAR(50) COMMENT '1、每次扣X元；2、按日薪/分钟 * 迟到的分钟数（不足一分按一分算），最低扣 X 元；',
   PARAMETERVALUE       NUMERIC(8,0) COMMENT '最低扣款',
   FINERATIO            NUMERIC(8,0) COMMENT '扣款系数',
   LOWESTTIMES          NUMERIC(8,0) COMMENT '最低次数',
   HIGHESTTIMES         NUMERIC(8,0) COMMENT '最高次数',
   FINESUM              NUMERIC(8,0) COMMENT '扣款额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (DEDUCTDETAILID)
);

ALTER TABLE T_HR_ATTENDANCEDEDUCTDETAIL COMMENT '考勤异常扣款明细';

/*==============================================================*/
/* Table: T_HR_ATTENDANCEDEDUCTMASTER                           */
/*==============================================================*/
CREATE TABLE T_HR_ATTENDANCEDEDUCTMASTER
(
   DEDUCTMASTERID       VARCHAR(50) NOT NULL COMMENT '考勤异常扣款主表ID',
   ATTENDABNORMALNAME   VARCHAR(50) COMMENT '异常扣款名',
   ATTENDABNORMALTYPE   VARCHAR(1) COMMENT '0迟到,1 早退,2未刷卡,3旷工',
   FINETYPE             VARCHAR(50) COMMENT '1、每次扣X元；2、按日薪/分钟 * 迟到的分钟数（不足一分按一分算），最低扣 X 元；3分段扣款',
   PARAMETERVALUE       NUMERIC(8,0) COMMENT '最低扣款',
   FINERATIO            NUMERIC(8,0) COMMENT '扣款系数',
   ISACCUMULATING       VARCHAR(50) COMMENT '是否累加',
   FINESUM              NUMERIC(8,0) COMMENT '扣款额',
   ISPERFECTATTENDANCEFACTOR VARCHAR(1) COMMENT '是否扣全勤',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (DEDUCTMASTERID)
);

ALTER TABLE T_HR_ATTENDANCEDEDUCTMASTER COMMENT '考勤异常扣款主表#';

/*==============================================================*/
/* Table: T_HR_ATTENDANCERECORD                                 */
/*==============================================================*/
CREATE TABLE T_HR_ATTENDANCERECORD
(
   ATTENDANCERECORDID   VARCHAR(50) NOT NULL COMMENT '考勤记录编号',
   ATTENDANCESOLUTIONID VARCHAR(50) COMMENT '考勤方案ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   WORKTIMESETID        VARCHAR(50) COMMENT '班次ID',
   ATTENDANCEDATE       DATETIME COMMENT '日期',
   FIRSTSTARTTIME       VARCHAR(50) COMMENT '第一段上班开始时间',
   FIRSTENDTIME         VARCHAR(50) COMMENT '第一段上班结束时间',
   FIRSTSTARTSTATE      VARCHAR(50) COMMENT '第一段上班开始状态',
   FIRSTENDSTATE        VARCHAR(50) COMMENT '第一段上班结束状态',
   SECONDSTARTTIME      VARCHAR(50) COMMENT '第二段上班开始时间',
   SECONDENDTIME        VARCHAR(50) COMMENT '第二段上班结束时间',
   SECONDSTARTSTATE     VARCHAR(50) COMMENT '第二段上班开始状态',
   SECONDENDSTATE       VARCHAR(50) COMMENT '第二段上班结束状态',
   THIRDSTARTTIME       VARCHAR(50) COMMENT '第三段上班开始时间',
   THIRDENDTIME         VARCHAR(50) COMMENT '第三段上班结束时间',
   THIRDSTARTSTATE      VARCHAR(50) COMMENT '第三段上班开始状态',
   THIRDENDSTATE        VARCHAR(50) COMMENT '第三段上班结束状态',
   FOURTHENDTIME        VARCHAR(50) COMMENT '第四段上班结束时间',
   FOURTHSTARTTIME      VARCHAR(50) COMMENT '第四段上班开始时间',
   FOURTHSTARTSTATE     VARCHAR(50) COMMENT '第四段上班开始状态',
   FOURTHENDSTATE       VARCHAR(50) COMMENT '第四段上班结束状态',
   ATTENDANCESTATE      VARCHAR(50) COMMENT '0,正常，1异常，2旷工，3出差，4休息，5请假',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   REMARK               VARCHAR(2000) COMMENT '备注',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   NEEDFRISTATTEND      VARCHAR(50) COMMENT '第一段是否上班考勤： 0  否,1 是',
   NEEDSECONDATTEND     VARCHAR(50) COMMENT '第二段是否上班考勤：0 上午,1 下午',
   NEEDTHIRDATTEND      VARCHAR(50) COMMENT '第三段是否上班考勤：0 上午,1 下午',
   NEEDFOURTHATTEND     VARCHAR(50) COMMENT '第四段是否上班考勤：0 上午,1 下午',
   PRIMARY KEY (ATTENDANCERECORDID)
);

ALTER TABLE T_HR_ATTENDANCERECORD COMMENT '考勤记录表';

/*==============================================================*/
/* Index: IDX_T_HR_ATTENDAN                                     */
/*==============================================================*/
CREATE INDEX IDX_T_HR_ATTENDAN ON T_HR_ATTENDANCERECORD
(
   EMPLOYEEID,
   ATTENDANCEDATE,
   OWNERCOMPANYID
);

/*==============================================================*/
/* Table: T_HR_ATTENDANCESOLUTION                               */
/*==============================================================*/
CREATE TABLE T_HR_ATTENDANCESOLUTION
(
   ATTENDANCESOLUTIONID VARCHAR(50) NOT NULL COMMENT '考勤方案ID',
   ATTENDANCESOLUTIONNAME VARCHAR(50) COMMENT '考勤方案名称',
   OVERTIMEREWARDID     VARCHAR(50) COMMENT '加班报酬ID',
   TEMPLATEMASTERID     VARCHAR(50) COMMENT '排班模板主表ID',
   ATTENDANCETYPE       VARCHAR(50) COMMENT '0 打卡考勤
            1 不需打卡考勤
            2 登录系统
            3 打卡加登录系统
            ',
   WORKDAYTYPE          VARCHAR(36) COMMENT '工作天数计算方式：
            0:固定方式；
            1：按当月实际工作日计',
   WORKDAYS             NUMERIC(8,0) COMMENT '固定方式下每月工作天数',
   WORKMODE             NUMERIC(8,0) COMMENT '工作制',
   CARDTYPE             VARCHAR(50) COMMENT '0 指纹打卡
            1 手动签卡
            2 电子卡',
   ONEDAYOVERTIMEHOURS  NUMERIC(8,0) COMMENT '加班调休一天小时数',
   ALLOWLOSTCARDTIMES   NUMERIC(8,0) COMMENT '每月允许漏打卡次数',
   ALLOWLATEMAXMINUTE   NUMERIC(8,0) COMMENT '每月允许迟到总时间',
   ALLOWLATEMAXTIMES    NUMERIC(8,0) COMMENT '每月允许迟到次数',
   ISCURRENTMONTH       VARCHAR(1) COMMENT '是否当月核算',
   SETTLEMENTDATE       VARCHAR(50) COMMENT '考勤核算日',
   ALARMDATE            VARCHAR(50) COMMENT '提醒通知日',
   ANNUALLEAVESET       VARCHAR(50) COMMENT '年假休假方式:
            0 一次性休完，此时不得用于冲减病事假；
            1、分N次修完，每冲减一次病事假算一次；
            2、无限制，可用于冲减病事假；',
   WORKTIMEPERDAY       NUMERIC(8,0) COMMENT '每天工作时长',
   ISEXPIRED            VARCHAR(1) COMMENT '0:不过期；1：过期；',
   ADJUSTEXPIREDVALUE   NUMERIC(8,0) COMMENT '调休过期时长',
   OVERTIMEPAYTYPE      VARCHAR(1) COMMENT '加班报酬方式：0 调休方式、1 加班工资方式、2 先调休再付工资方式、3 无报酬方式；',
   OVERTIMEVALID        VARCHAR(1) COMMENT '加班生效方式
            0 审核通过的加班申请；1 超过工作时间外自动累计；2 仅节假日累计；',
   OVERTIMECHECK        VARCHAR(1) COMMENT '加班是否要打卡',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   YEARLYBALANCETYPE    VARCHAR(1) COMMENT '考情结算方式:0清空,1累计到下一年,2折算成工资',
   YEARLYBALANCEDATE    VARCHAR(50) COMMENT '年度考勤结算日',
   ISAUTOIMPORTPUNCH    VARCHAR(10) COMMENT '是否自动导入打卡记录',
   YOUTHEXTEND          VARCHAR(1) COMMENT '0:不延长；1：延长；',
   YOUTHEXTENDVALUE     NUMERIC(8,0) COMMENT '五四三八假期延期时长',
   AUTOLEFTOFFICERECEIVEPOST VARCHAR(2000) COMMENT '自动离职处理岗位',
   LEFTOFFICERECEIVEPOSTNAME VARCHAR(2000) COMMENT '自动离职处理岗位名称',
   PRIMARY KEY (ATTENDANCESOLUTIONID)
);

ALTER TABLE T_HR_ATTENDANCESOLUTION COMMENT '考勤方案#';

/*==============================================================*/
/* Table: T_HR_ATTENDANCESOLUTIONASIGN                          */
/*==============================================================*/
CREATE TABLE T_HR_ATTENDANCESOLUTIONASIGN
(
   ATTENDANCESOLUTIONASIGNID VARCHAR(50) NOT NULL COMMENT '员工考勤方案ID',
   ATTENDANCESOLUTIONID VARCHAR(50) COMMENT '考勤方案ID',
   ASSIGNEDOBJECTID     VARCHAR(2000) COMMENT '分配对像ID',
   ASSIGNEDOBJECTTYPE   VARCHAR(50) COMMENT '分配对像类别',
   STARTDATE            DATETIME COMMENT '有效期开始时间',
   ENDDATE              DATETIME COMMENT '有效期结束时间',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   PRIMARY KEY (ATTENDANCESOLUTIONASIGNID)
);

ALTER TABLE T_HR_ATTENDANCESOLUTIONASIGN COMMENT '考勤方案应用';

/*==============================================================*/
/* Table: T_HR_ATTENDANCESOLUTIONDEDUCT                         */
/*==============================================================*/
CREATE TABLE T_HR_ATTENDANCESOLUTIONDEDUCT
(
   SOLUTIONDEDUCTID     VARCHAR(50) NOT NULL COMMENT '考勤方案异常扣款ID',
   ATTENDANCESOLUTIONID VARCHAR(50) COMMENT '考勤方案ID',
   DEDUCTMASTERID       VARCHAR(50) COMMENT '考勤异常扣款主表ID',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (SOLUTIONDEDUCTID)
);

ALTER TABLE T_HR_ATTENDANCESOLUTIONDEDUCT COMMENT '考勤方案异常扣款';

/*==============================================================*/
/* Table: T_HR_BALANCEPOSTDETAIL                                */
/*==============================================================*/
CREATE TABLE T_HR_BALANCEPOSTDETAIL
(
   BALANCEPOSTDETAIL    VARCHAR(50) NOT NULL COMMENT '明细ID',
   BALANCEPOSTASIGNID   VARCHAR(50) COMMENT '变更申请ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEEPOSTID       VARCHAR(50) COMMENT '岗位ID',
   EMPLOYEEPOSTNAME     VARCHAR(50) COMMENT '岗位名称',
   EMPLOYEEDEPARTMENTID VARCHAR(50) COMMENT '部门ID',
   EMPLOYEEDEPARTMENTNAME VARCHAR(50) COMMENT '部门名称',
   EMPLOYEECOMPANYID    VARCHAR(50) COMMENT '公司ID',
   EMPLOYEECOMPANYNAME  VARCHAR(50) COMMENT '公司名称',
   CREATEDATE           DATETIME COMMENT '创建时间',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人ID',
   UPDATEDATE           DATETIME COMMENT '更新时间',
   UPDATEDATEUSERID     VARCHAR(50) COMMENT '更新人ID',
   EDITSTATE            VARCHAR(1) DEFAULT '0' COMMENT '生效状态',
   ATTENDANCESET        VARCHAR(1) DEFAULT '0' COMMENT '考勤设置',
   SALARYSET            VARCHAR(1) DEFAULT '0' COMMENT '薪资结算设置',
   PRIMARY KEY (BALANCEPOSTDETAIL)
);

ALTER TABLE T_HR_BALANCEPOSTDETAIL COMMENT '结算岗位明细';

/*==============================================================*/
/* Table: T_HR_BLACKLIST                                        */
/*==============================================================*/
CREATE TABLE T_HR_BLACKLIST
(
   BLACKLISTID          VARCHAR(50) NOT NULL COMMENT '黑名单ID',
   IDCARDNUMBER         VARCHAR(50) NOT NULL COMMENT '身份证号码',
   NAME                 VARCHAR(50) NOT NULL COMMENT '姓名',
   REASON               VARCHAR(2000) COMMENT '原因',
   BEGINDATE            VARCHAR(50) COMMENT '起始时间',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CHECKSTATE           VARCHAR(50) COMMENT '审核状态',
   EDITSTATE            VARCHAR(50) COMMENT '编辑状态',
   EFFECTIVETIME        NUMERIC(8,0) COMMENT '有效时间',
   PRIMARY KEY (BLACKLISTID)
);

ALTER TABLE T_HR_BLACKLIST COMMENT '黑名单(未加权限)';

/*==============================================================*/
/* Table: T_HR_CHECKCATEGORYSET                                 */
/*==============================================================*/
CREATE TABLE T_HR_CHECKCATEGORYSET
(
   CHECKCATEGORYID      VARCHAR(50) NOT NULL,
   CHECKCATEGORYNAME    VARCHAR(500),
   DESCRIPTION          VARCHAR(2000),
   CHECKTYPE            VARCHAR(1) COMMENT '考核方式：0，处理时间，1，手工评估，如果是手工评估那么评分方式不能选择机打',
   SCORETYPE            VARCHAR(1) COMMENT '评分方式：分为系统打分（系统通过处理流程步骤的时间来自动算分），手工打分（流程下一步骤负责人为流程上一步骤负责人打分），抽查打分（从设定的抽查组中随机查出一人来给此流程步骤打分）三种 ',
   REMARK               VARCHAR(2000),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME,
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (CHECKCATEGORYID)
);

ALTER TABLE T_HR_CHECKCATEGORYSET COMMENT '考核类别设置';

/*==============================================================*/
/* Table: T_HR_CHECKMODELDEFINE                                 */
/*==============================================================*/
CREATE TABLE T_HR_CHECKMODELDEFINE
(
   CHECKMODELID         VARCHAR(50) NOT NULL,
   ORGCHECKMODELID      VARCHAR(50),
   PERFORMANCEGOAL      VARCHAR(2000),
   WEIGHING             NUMERIC(8,0),
   POINT                NUMERIC(8,0),
   ISFLOWMODEL          VARCHAR(1),
   FLOWID               VARCHAR(50),
   REMARK               VARCHAR(2000),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME,
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (CHECKMODELID)
);

ALTER TABLE T_HR_CHECKMODELDEFINE COMMENT '考核项目设置';

/*==============================================================*/
/* Table: T_HR_CHECKPOINTLEVELSET                               */
/*==============================================================*/
CREATE TABLE T_HR_CHECKPOINTLEVELSET
(
   POINTLEVEID          VARCHAR(50) NOT NULL COMMENT '项目明细等级ID',
   CHECKPOINTSETID      VARCHAR(50) COMMENT '考核项目点ID',
   POINTLEVEL           VARCHAR(50) COMMENT '项目细分点等级名',
   POINTSCORE           NUMERIC(8,0) COMMENT '等级分数',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (POINTLEVEID)
);

ALTER TABLE T_HR_CHECKPOINTLEVELSET COMMENT '考核项目等级';

/*==============================================================*/
/* Table: T_HR_CHECKPOINTSET                                    */
/*==============================================================*/
CREATE TABLE T_HR_CHECKPOINTSET
(
   CHECKPOINTSETID      VARCHAR(50) NOT NULL COMMENT '考核项目点ID',
   CHECKPROJECTID       VARCHAR(50) COMMENT '考核项目ID',
   CHECKEMPLOYEETYPE    VARCHAR(1) COMMENT '考核员工类型',
   CHECKPOINT           VARCHAR(50) COMMENT '考核项目点',
   CHECKPOINTDES        VARCHAR(50) COMMENT '考核项目点描述',
   CHECKPOINTSCORE      NUMERIC(8,0) COMMENT '考核项目点权重',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (CHECKPOINTSETID)
);

ALTER TABLE T_HR_CHECKPOINTSET COMMENT '人事考核项目点设置';

/*==============================================================*/
/* Table: T_HR_CHECKPROJECTSET                                  */
/*==============================================================*/
CREATE TABLE T_HR_CHECKPROJECTSET
(
   CHECKPROJECTID       VARCHAR(50) NOT NULL COMMENT '考核项目ID',
   CHECKPROJECT         VARCHAR(50) COMMENT '考核项目名',
   CHECKPROJECTSCORE    NUMERIC(8,0) COMMENT '项目分数',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (CHECKPROJECTID)
);

ALTER TABLE T_HR_CHECKPROJECTSET COMMENT '人事考核项目设置';

/*==============================================================*/
/* Table: T_HR_COMPANY                                          */
/*==============================================================*/
CREATE TABLE T_HR_COMPANY
(
   COMPANYID            VARCHAR(50) NOT NULL COMMENT '公司ID',
   COMPANYTYPE          VARCHAR(50) COMMENT '企业类型:1地产企业,2物流企业,3生产企业,4软件企业,零售企业',
   COMPANRYCODE         VARCHAR(50) COMMENT '公司编号',
   ENAME                VARCHAR(100) COMMENT '公司英文名称',
   CNAME                VARCHAR(100) COMMENT '公司中文名称',
   COMPANYCATEGORY      VARCHAR(50) COMMENT '公司类型',
   CITY                 VARCHAR(50) COMMENT '所在地城市，系统字典中定义',
   COUNTYTYPE           VARCHAR(50) COMMENT '0 中国大陆
            1 中国香港
            系统字典中定义',
   COMPANYLEVEL         VARCHAR(50) COMMENT '公司级别',
   FATHERCOMPANYID      VARCHAR(50) COMMENT '父公司ID',
   ADDRESS              VARCHAR(100) COMMENT '公司地址',
   LEGALPERSON          VARCHAR(100) COMMENT '法人代表',
   LINKMAN              VARCHAR(50) COMMENT '联系人姓名',
   TELNUMBER            VARCHAR(50) COMMENT '联系电话',
   LEGALPERSONID        VARCHAR(50) COMMENT '法人身份证号',
   BUSSINESSLICENCENO   VARCHAR(100) COMMENT '营业执照号',
   BUSSINESSAREA        VARCHAR(1000) COMMENT '经营范围',
   ACCOUNTCODE          VARCHAR(100) COMMENT '银行账号',
   BANKID               VARCHAR(100) COMMENT '开户银行代码',
   EMAIL                VARCHAR(50) COMMENT '电子邮件',
   ZIPCODE              VARCHAR(50) COMMENT '邮政编码',
   FAXNUMBER            VARCHAR(50) COMMENT '传真',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   FATHERID             VARCHAR(50) COMMENT '上级机构ID',
   FATHERTYPE           VARCHAR(50) COMMENT '上级机构类型',
   REMARK               VARCHAR(2000) COMMENT '备注',
   SORTINDEX            NUMERIC(8,0) COMMENT '排序号',
   BRIEFNAME            VARCHAR(100) COMMENT '简称',
   PRIMARY KEY (COMPANYID)
);

ALTER TABLE T_HR_COMPANY COMMENT '公司#';

/*==============================================================*/
/* Table: T_HR_COMPANYHISTORY                                   */
/*==============================================================*/
CREATE TABLE T_HR_COMPANYHISTORY
(
   RECORDSID            VARCHAR(50) NOT NULL COMMENT '记录ID',
   COMPANYID            VARCHAR(50) NOT NULL COMMENT '公司ID',
   COMPANRYCODE         VARCHAR(50) COMMENT '公司编号',
   ENAME                VARCHAR(100) COMMENT '公司英文名称',
   CNAME                VARCHAR(100) COMMENT '公司中文名称',
   COMPANYCATEGORY      VARCHAR(50) COMMENT '公司类型',
   COMPANYLEVEL         VARCHAR(50) COMMENT '公司级别',
   FATHERCOMPANYID      VARCHAR(50) COMMENT '父公司ID',
   LEGALPERSON          VARCHAR(100) COMMENT '法人代表',
   LINKMAN              VARCHAR(50) COMMENT '联系人姓名',
   TELNUMBER            VARCHAR(50) COMMENT '联系电话',
   ADDRESS              VARCHAR(100) COMMENT '公司地址',
   LEGALPERSONID        VARCHAR(50) COMMENT '法人身份证号',
   BUSSINESSLICENCENO   VARCHAR(100) COMMENT '营业执照号',
   BUSSINESSAREA        VARCHAR(1000) COMMENT '经营范围',
   ACCOUNTCODE          VARCHAR(100) COMMENT '银行账号',
   BANKID               VARCHAR(100) COMMENT '开户银行代码',
   EMAIL                VARCHAR(50) COMMENT '电子邮件',
   ZIPCODE              VARCHAR(50) COMMENT '邮政编码',
   FAXNUMBER            VARCHAR(50) COMMENT '传真',
   REUSEDATE            DATETIME COMMENT '生效时间',
   CANCELDATE           DATETIME COMMENT '撤销时间',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CITY                 VARCHAR(50) COMMENT '所在地城市，系统字典中定义',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   FATHERID             VARCHAR(50) COMMENT '上级机构ID',
   FATHERTYPE           VARCHAR(50) COMMENT '上级机构类型',
   PRIMARY KEY (RECORDSID)
);

ALTER TABLE T_HR_COMPANYHISTORY COMMENT '公司历史表#';

/*==============================================================*/
/* Table: T_HR_CUSTOMGUERDON                                    */
/*==============================================================*/
CREATE TABLE T_HR_CUSTOMGUERDON
(
   CUSTOMGUERDONID      VARCHAR(50) NOT NULL COMMENT '自定义薪资ID',
   SALARYSTANDARDID     VARCHAR(50) COMMENT '薪资标准ID',
   CUSTOMGUERDONSETID   VARCHAR(50) COMMENT '自定义薪资设置ID',
   CALCULATEFORMULA     VARCHAR(2000) COMMENT '计算公式',
   SUM                  NUMERIC(8,0) COMMENT '金额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (CUSTOMGUERDONID)
);

ALTER TABLE T_HR_CUSTOMGUERDON COMMENT '自定义薪资';

/*==============================================================*/
/* Table: T_HR_CUSTOMGUERDONARCHIVE                             */
/*==============================================================*/
CREATE TABLE T_HR_CUSTOMGUERDONARCHIVE
(
   CUSTOMGUERDONARCHIVEID VARCHAR(50) NOT NULL COMMENT '自定义薪资档案ID',
   SALARYARCHIVEID      VARCHAR(50) COMMENT '薪资档案ID',
   CUSTOMERGUERDONID    VARCHAR(50) COMMENT '自定义薪资设置ID',
   SUM                  NUMERIC(8,0) COMMENT '金额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   ASSIGNERID           VARCHAR(50) COMMENT '经费下拨人员工ID',
   ASSIGNERPOSTID       VARCHAR(50) COMMENT '经费下拨人岗位ID',
   ASSIGNERDEPARTMENTID VARCHAR(50) COMMENT '经费下拨人部门ID',
   ASSIGNERRCOMPANYID   VARCHAR(50) COMMENT '经费下拨人公司ID',
   ASSIGNERNAME         VARCHAR(50) COMMENT '经费下拨人员工姓名',
   ASSIGNERPOSTNAME     VARCHAR(100) COMMENT '经费下拨人岗位名称',
   ASSIGNERDEPARTMENTNAME VARCHAR(100) COMMENT '经费下拨人部门名称',
   ASSIGNERRCOMPANYNAME VARCHAR(100) COMMENT '经费下拨人公司名称',
   PRIMARY KEY (CUSTOMGUERDONARCHIVEID)
);

ALTER TABLE T_HR_CUSTOMGUERDONARCHIVE COMMENT '自定义薪资档案';

/*==============================================================*/
/* Table: T_HR_CUSTOMGUERDONARCHIVEHIS                          */
/*==============================================================*/
CREATE TABLE T_HR_CUSTOMGUERDONARCHIVEHIS
(
   CUSTOMGUERDONARCHIVEID VARCHAR(50) NOT NULL COMMENT '自定义薪资历史ID',
   SALARYARCHIVEID      VARCHAR(50) COMMENT '薪资档案ID',
   CUSTOMERGUERDONID    VARCHAR(50) COMMENT '自定义薪资设置ID',
   SUM                  NUMERIC(8,0) COMMENT '金额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (CUSTOMGUERDONARCHIVEID)
);

ALTER TABLE T_HR_CUSTOMGUERDONARCHIVEHIS COMMENT '自定义薪资档案历史';

/*==============================================================*/
/* Table: T_HR_CUSTOMGUERDONRECORD                              */
/*==============================================================*/
CREATE TABLE T_HR_CUSTOMGUERDONRECORD
(
   CUSTOMGUERDONRECORDID VARCHAR(50) NOT NULL COMMENT '自定义薪资发放ID',
   EMPLOYEEID           VARCHAR(50) NOT NULL COMMENT '员工ID',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   SALARYMONTH          VARCHAR(50) COMMENT '所属月份',
   SALARYYEAR           VARCHAR(50) COMMENT '所属年份',
   GENERATETYPE         VARCHAR(50) COMMENT '0自动生成的
            1手动添加的',
   CUSTOMERGUERDONID    VARCHAR(50) COMMENT '自定义薪资项ID',
   GUERDONNAME          VARCHAR(100) COMMENT '自定义薪资项名称',
   SALARYSUM            NUMERIC(8,0) COMMENT '薪资金额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   CHECKSTATE           VARCHAR(50) COMMENT '审核状态',
   PRIMARY KEY (CUSTOMGUERDONRECORDID)
);

ALTER TABLE T_HR_CUSTOMGUERDONRECORD COMMENT '自定义薪资记录';

/*==============================================================*/
/* Table: T_HR_CUSTOMGUERDONSET                                 */
/*==============================================================*/
CREATE TABLE T_HR_CUSTOMGUERDONSET
(
   CUSTOMGUERDONSETID   VARCHAR(50) NOT NULL COMMENT '自定义薪资设置ID',
   GUERDONNAME          VARCHAR(50) COMMENT '自定义薪资项名称',
   GUERDONCATEGORY      VARCHAR(50) COMMENT '指定项目的属性（加或减）',
   CALCULATORTYPE       VARCHAR(50) COMMENT '1、与系统中定义的项目之间的计算公式；
            2、薪资档案中输入；
            3、手工录入；',
   GUERDONSUM           NUMERIC(8,0) COMMENT '金额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   PRIMARY KEY (CUSTOMGUERDONSETID)
);

ALTER TABLE T_HR_CUSTOMGUERDONSET COMMENT '自定义薪资设置';

/*==============================================================*/
/* Table: T_HR_DEPARTMENT                                       */
/*==============================================================*/
CREATE TABLE T_HR_DEPARTMENT
(
   DEPARTMENTID         VARCHAR(50) NOT NULL COMMENT '部门ID',
   COMPANYID            VARCHAR(50) COMMENT '公司ID',
   DEPARTMENTDICTIONARYID VARCHAR(50) COMMENT '部门字典ID',
   DEPARTMENTFUNCTION   VARCHAR(2000) COMMENT '部门职责',
   CHECKSTATE           VARCHAR(1) COMMENT '0未提交 1审核中 2已审核 3审核不通过',
   EDITSTATE            VARCHAR(1) COMMENT '0未生效 1生效 2撤销中 3已撤销',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   FATHERID             VARCHAR(50) COMMENT '上级机构ID',
   FATHERTYPE           VARCHAR(50) COMMENT '上级机构类型',
   DEPARTMENTBOSSHEAD   VARCHAR(50) COMMENT '部门负责人(员工ID)',
   REMARK               VARCHAR(2000) COMMENT '备注',
   SORTINDEX            NUMERIC(8,0) COMMENT '排序号',
   ISBACKGROUND         NUMERIC(8,0) COMMENT '是否前后台：0后台部门，1前台部门',
   CENTERHEAD           VARCHAR(100),
   DETPMANAGER          VARCHAR(100),
   PRIMARY KEY (DEPARTMENTID)
);

ALTER TABLE T_HR_DEPARTMENT COMMENT '部门#';

/*==============================================================*/
/* Table: T_HR_DEPARTMENTDICTIONARY                             */
/*==============================================================*/
CREATE TABLE T_HR_DEPARTMENTDICTIONARY
(
   DEPARTMENTDICTIONARYID VARCHAR(50) NOT NULL COMMENT '部门字典ID',
   DEPARTMENTTYPE       VARCHAR(10) COMMENT '部门类别:0通用部门,1地产企业部门,2物流企业部门,3生产企业部门,4软件企业部门,零售企业部门',
   DEPARTMENTCODE       VARCHAR(50) COMMENT '部门编号',
   DEPARTMENTNAME       VARCHAR(50) COMMENT '部门名称',
   DEPARTMENTFUNCTION   VARCHAR(2000) COMMENT '部门职责',
   DEPARTMENTLEVEL      VARCHAR(50) COMMENT '部门级别',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   CHECKSTATE           VARCHAR(50) COMMENT '审核状态',
   EDITSTATE            VARCHAR(50) COMMENT '编辑状态',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   PRIMARY KEY (DEPARTMENTDICTIONARYID)
);

ALTER TABLE T_HR_DEPARTMENTDICTIONARY COMMENT '部门字典';

/*==============================================================*/
/* Table: T_HR_DEPARTMENTHISTORY                                */
/*==============================================================*/
CREATE TABLE T_HR_DEPARTMENTHISTORY
(
   RECORDSID            VARCHAR(50) NOT NULL COMMENT '记录ID',
   DEPARTMENTDICTIONARYID VARCHAR(50) COMMENT '部门字典ID',
   DEPARTMENTID         VARCHAR(50) COMMENT '部门ID',
   COMPANYID            VARCHAR(50) COMMENT '公司ID',
   DEPARTMENTFUNCTION   VARCHAR(2000) COMMENT '部门职责',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   CANCELDATE           DATETIME COMMENT '撤销时间',
   REUSEDATE            DATETIME COMMENT '生效时间',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   FATHERTYPE           VARCHAR(1) COMMENT '上级机构类型',
   FATHERID             VARCHAR(50) COMMENT '上级机构ID',
   DEPARTMENTBOSSHEAD   VARCHAR(50) COMMENT '部门负责人的员工ID',
   CHECKSTATE           VARCHAR(50) COMMENT '审核状态',
   PRIMARY KEY (RECORDSID)
);

ALTER TABLE T_HR_DEPARTMENTHISTORY COMMENT '部门历史记录#';

/*==============================================================*/
/* Table: T_HR_EMPLOYEE                                         */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEE
(
   EMPLOYEEID           VARCHAR(50) NOT NULL COMMENT '员工ID',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   EMPLOYEECNAME        VARCHAR(50) COMMENT '员工中文名',
   EMPLOYEEENAME        VARCHAR(50) COMMENT '员工英文名',
   EMPLOYEELEVEL        VARCHAR(50) COMMENT '员工等级',
   SEX                  VARCHAR(1) COMMENT '员工性别：0 女，1 男',
   PROFESSIONALTITLE    VARCHAR(50) COMMENT '职称',
   IDTYPE               VARCHAR(50) COMMENT '证件类型',
   IDNUMBER             VARCHAR(50) COMMENT '证件号码',
   NATION               VARCHAR(50) COMMENT '民族',
   HEIGHT               VARCHAR(50) COMMENT '身高',
   BANKID               VARCHAR(50) COMMENT '开户银行',
   BANKCARDNUMBER       VARCHAR(50) COMMENT '银行帐号',
   RESUMEID             VARCHAR(50) COMMENT '简历id',
   PROVINCE             VARCHAR(100) COMMENT '籍贯',
   BLOODTYPE            VARCHAR(50) COMMENT '血型',
   MARRIAGE             VARCHAR(50) COMMENT '婚姻状况',
   HASCHILDREN          VARCHAR(50) COMMENT '有无子女',
   POLITICS             VARCHAR(100) COMMENT '政治面貌',
   REGRESIDENCE         VARCHAR(200) COMMENT '户口所在地',
   REGRESIDENCETYPE     VARCHAR(50) COMMENT '户口类型：省外市辖镇',
   RESIDENCETYPE        VARCHAR(50) COMMENT '户籍类型：
            0 非农业家庭户口
            1 非农业集体户口
            2 非农业空挂户口
            3 农业户口',
   EMAIL                VARCHAR(50) COMMENT '电子邮件',
   MOBILE               VARCHAR(50) COMMENT '手机号码',
   SECONDLANGUAGE       VARCHAR(50) COMMENT '语种',
   SECONDLANGUAGEDEGREE VARCHAR(50) COMMENT '语种熟练程度',
   OFFICEPHONE          VARCHAR(50) COMMENT '办公电话',
   CURRENTADDRESS       VARCHAR(100) COMMENT '目前居住地',
   URGENCYPERSON        VARCHAR(50) COMMENT '紧急联系人',
   URGENCYCONTACT       VARCHAR(50) COMMENT '紧急联系方式',
   FAMILYADDRESS        VARCHAR(100) COMMENT '家庭详细地址',
   FAMILYZIPCODE        VARCHAR(100) COMMENT '家庭邮政编码',
   FAMILYPHONE          VARCHAR(50) COMMENT '家庭电话',
   FINGERPRINTID        VARCHAR(50) COMMENT '指纹编号',
   PHOTO                LONGBLOB COMMENT '照片',
   TOPEDUCATION         VARCHAR(100) COMMENT '最高学历',
   BIRTHDAY             DATETIME COMMENT '出生日期',
   EMPLOYEESTATE        VARCHAR(50) COMMENT '员工状态：0试用 1在职 2已离职 3离职中',
   WORKINGAGE           NUMERIC(8,0) COMMENT '社会工龄',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   EMPLOYEETYPE         VARCHAR(1) COMMENT '员工类型',
   OTHERCOMMUNICATE     VARCHAR(500) COMMENT '其他联系方式:RTX,QQ',
   INTERESTCONTENT      VARCHAR(500) COMMENT '爱好/特长',
   SOCIALSERVICEYEAR    VARCHAR(50) COMMENT '社保缴纳起始时间',
   FATHEREMPLOYEEID     VARCHAR(50),
   RECRUITMENTSOURCE    VARCHAR(100),
   JOBCATEGORY          VARCHAR(100),
   CADREINFORMATION     VARCHAR(100),
   SILING               VARCHAR(100),
   PLATFORM             VARCHAR(2000),
   JOINDATE             DATETIME,
   PRIMARY KEY (EMPLOYEEID)
);

ALTER TABLE T_HR_EMPLOYEE COMMENT '员工基本信息#';

/*==============================================================*/
/* Table: T_HR_EMPLOYEECHANGEHISTORY                            */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEECHANGEHISTORY
(
   RECORDID             VARCHAR(50) NOT NULL COMMENT '记录id',
   EMPLOYEEID           VARCHAR(50) NOT NULL COMMENT '员工ID',
   EMPOLYEENAME         VARCHAR(50) COMMENT '员工姓名',
   FINGERPRINTID        VARCHAR(50) COMMENT '指纹编号',
   FORMTYPE             VARCHAR(50) COMMENT '0.入职
            1.异动
            2.离职
            3.薪资级别变更
            4.签订合同',
   FORMID               VARCHAR(50) NOT NULL COMMENT '记录原始单据id',
   ISMASTERPOSTCHANGE   VARCHAR(50) COMMENT '0 主岗位
            1 非主岗位',
   CHANGETYPE           VARCHAR(50) COMMENT '包括 异动类型及离职类型：',
   CHANGETIME           DATETIME COMMENT '异动时间',
   CHANGEREASON         VARCHAR(2000) COMMENT '异动原因',
   OLDPOSTID            VARCHAR(50) COMMENT '异动前岗位id',
   OLDPOSTNAME          VARCHAR(100) COMMENT '异动前岗位名称',
   OLDPOSTLEVEL         VARCHAR(50) COMMENT '异动前岗位级别',
   OLDSALARYLEVEL       VARCHAR(50) COMMENT '异动前薪资级别',
   OLDDEPARTMENTID      VARCHAR(50) COMMENT '异动前部门id',
   OLDDEPARTMENTNAME    VARCHAR(100) COMMENT '异动前部门名称',
   OLDCOMPANYID         VARCHAR(50) COMMENT '异动前公司id',
   OLDCOMPANYNAME       VARCHAR(100) COMMENT '异动前公司名称',
   OLDSALARYSUM         VARCHAR(50) COMMENT '异动前薪资额度',
   NEXTPOSTID           VARCHAR(50) COMMENT '异动后岗位id',
   NEXTPOSTNAME         VARCHAR(100) COMMENT '异动后岗位名称',
   NEXTPOSTLEVEL        VARCHAR(50) COMMENT '异动后岗位级别',
   NEXTSALARYLEVEL      VARCHAR(50) COMMENT '异动后薪资级别',
   NEXTDEPARTMENTID     VARCHAR(50) COMMENT '异动后部门id',
   NEXTDEPARTMENTNAME   VARCHAR(100) COMMENT '异动后部门名称',
   NEXTCOMPANYID        VARCHAR(50) COMMENT '异动后公司id',
   NEXTCOMPANYNAME      VARCHAR(100) COMMENT '异动后公司名称',
   REMART               VARCHAR(2000) COMMENT '备注',
   CREATEDATE           DATETIME COMMENT '创建时间',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   PRIMARY KEY (RECORDID)
);

ALTER TABLE T_HR_EMPLOYEECHANGEHISTORY COMMENT '员工异动历史';

/*==============================================================*/
/* Table: T_HR_EMPLOYEEOUTAPPLIECRECORD                         */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEEOUTAPPLIECRECORD
(
   OVERTIMERECORDID     VARCHAR(50) NOT NULL COMMENT '外出申请记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   OVERTIMECATE         VARCHAR(50) COMMENT '加班生效方式
            0 审核通过的加班申请；1 超过工作时间外自动累计；2 仅节假日累计；',
   STARTDATE            DATETIME COMMENT '外出开始日期',
   ENDDATE              DATETIME COMMENT '外出结束日期',
   OVERTIMEHOURS        VARCHAR(50) COMMENT '外出时长',
   BEGINCARDTIME        VARCHAR(50) COMMENT '外出离开刷卡时间',
   BEGINCARDTYPE        VARCHAR(1) COMMENT '0,正常刷卡,1 签卡',
   ENDCARDTIME          VARCHAR(50) COMMENT '外出回来刷卡时间',
   ENDCARDTYPE          VARCHAR(1) COMMENT '0,正常刷卡,1 签卡',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (OVERTIMERECORDID)
);

ALTER TABLE T_HR_EMPLOYEEOUTAPPLIECRECORD COMMENT '员工外出申请记录';

/*==============================================================*/
/* Table: T_HR_EMPLOYEEPOST                                     */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEEPOST
(
   EMPLOYEEPOSTID       VARCHAR(50) NOT NULL COMMENT '员工岗位id',
   ISAGENCY             VARCHAR(1) COMMENT '是否代理:0非代理,1代理岗位',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   POSTID               VARCHAR(50) COMMENT '岗位ID',
   SALARYLEVEL          NUMERIC(8,0) COMMENT '薪资等级',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态:0未生效，1生效中',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   POSTLEVEL            NUMERIC(8,0) COMMENT '岗位等级',
   REPORTEMP            VARCHAR(2000) COMMENT '工作计划汇报上级',
   REPORTEMPNAME        VARCHAR(2000) COMMENT '工作计划汇报上级名称',
   PRIMARY KEY (EMPLOYEEPOSTID)
);

ALTER TABLE T_HR_EMPLOYEEPOST COMMENT '员工岗位';

/*==============================================================*/
/* Index: IDX_HRPOSTID                                          */
/*==============================================================*/
CREATE INDEX IDX_HRPOSTID ON T_HR_EMPLOYEEPOST
(
   
);

/*==============================================================*/
/* Table: T_HR_EMPLOYEEPOSTHISTORY                              */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEEPOSTHISTORY
(
   RECORDSID            VARCHAR(50) NOT NULL COMMENT '记录ID',
   EMPLOYEEPOSTID       VARCHAR(50) COMMENT '员工岗位id',
   POSTID               VARCHAR(50) COMMENT '岗位ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   SALARYLEVEL          NUMERIC(8,0) COMMENT '薪资等级',
   REASON               VARCHAR(2000) COMMENT '原因',
   PRIMARY KEY (RECORDSID)
);

ALTER TABLE T_HR_EMPLOYEEPOSTHISTORY COMMENT '员工岗位历史#';

/*==============================================================*/
/* Table: T_HR_EMPLOYEESALARYPOSTASIGN                          */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEESALARYPOSTASIGN
(
   EMPLOYEESALARYPOSTASIGNID VARCHAR(50) NOT NULL,
   BALANCEPOSTID        VARCHAR(50) COMMENT '结算岗位ID',
   BALANCEPOSTNAME      VARCHAR(200) COMMENT '结算岗位ID名称',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人ID',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人ID',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改人姓名',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   REMARK               VARCHAR(2000) COMMENT '备注',
   EMPLOYEECOUNT        NUMERIC(8,0) COMMENT '员工数量',
   NOTESCONTENT         VARCHAR(2000) COMMENT '说明',
   PRIMARY KEY (EMPLOYEESALARYPOSTASIGNID)
);

ALTER TABLE T_HR_EMPLOYEESALARYPOSTASIGN COMMENT 'T_HR_EMPLOYEESALARYPOSTASIGN';

/*==============================================================*/
/* Table: T_HR_EMPLOYEEVACATION                                 */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEEVACATION
(
   EMPLOYEEID           VARCHAR(50) NOT NULL,
   EMPLOYEECODE         VARCHAR(50) NOT NULL,
   EMPLOYEENAME         VARCHAR(50) NOT NULL,
   YEAR_PERIOD          VARCHAR(10) NOT NULL COMMENT '年份',
   ACCOMPANY_CARE_LEAVE NUMERIC(8,2) DEFAULT 0 COMMENT '陪护假',
   AFFAIR_LEAVE_HOURS   NUMERIC(8,2) DEFAULT 0 COMMENT '事假',
   ANNUAL_HOURS         NUMERIC(8,2) DEFAULT 0 COMMENT '年假小时数',
   ANNUALDAY            NUMERIC(8,2) DEFAULT 0 COMMENT '年假天数',
   ANNUALDAY_EFFECT_DAY NUMERIC(8,2) DEFAULT 0 COMMENT '年假生效日期',
   ANNUALDAY_USED       NUMERIC(8,2) DEFAULT 0 COMMENT '年假已用天数',
   ANNUALHOURS_USED     NUMERIC(8,2) DEFAULT 0 COMMENT '年假已用小时数',
   BREAST_FEEDING_LEAVE NUMERIC(8,2) DEFAULT 0 COMMENT '哺乳假',
   FUNERAL_LEAVE        NUMERIC(8,2) DEFAULT 0 COMMENT '丧假',
   INJURY_LEAVE         NUMERIC(8,2) DEFAULT 0 COMMENT '工伤假',
   MARRIAGE_LEAVE       NUMERIC(8,2) DEFAULT 0 COMMENT '婚假',
   MATERNITY_LEAVE      NUMERIC(8,2) DEFAULT 0 COMMENT '产假',
   OT_LEAVE_HOURS       NUMERIC(8,2) DEFAULT 0 COMMENT '调休小时数',
   OT_TOTAL_HOURS       NUMERIC(8,2) DEFAULT 0 COMMENT '加班小时数',
   PRENATAL_EXAM_LEAVE  NUMERIC(8,2) DEFAULT 0 COMMENT '产检假',
   ROADING_LEAVE        NUMERIC(8,2) DEFAULT 0 COMMENT '路程假',
   SICK_HOURS           NUMERIC(8,2) DEFAULT 0 COMMENT '病假每月小时数',
   SICK_LEAVE_HOURS     NUMERIC(8,2) DEFAULT 0 COMMENT '病假已休小时数',
   WOMEN_DAY            NUMERIC(8,2) DEFAULT 0 COMMENT '三八妇女节',
   YOUTH_DAY            NUMERIC(8,2) DEFAULT 0 COMMENT '五四青年节',
   CREATE_USERID        VARCHAR(50) NOT NULL,
   CREATE_DATE          DATETIME ,
   CREATE_COMPANYID     VARCHAR(50) NOT NULL,
   CREATE_DEPARTMENTID  VARCHAR(50) NOT NULL,
   CREATE_POSTID        VARCHAR(50) NOT NULL,
   OWNER_COMPANYID      VARCHAR(50) NOT NULL,
   OWNER_DEPARTMENTID   VARCHAR(50) NOT NULL,
   OWNER_POSTID         VARCHAR(50) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   UPDATE_USERID        VARCHAR(50) NOT NULL,
   UPDATE_DATE          DATETIME 
);

ALTER TABLE T_HR_EMPLOYEEVACATION COMMENT '员工带薪假期列表';

/*==============================================================*/
/* Table: T_HR_EMPLOYEE_LIUHH                                   */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEE_LIUHH
(
   EMPLOYEEID           VARCHAR(50) NOT NULL,
   EMPLOYEECODE         VARCHAR(50),
   EMPLOYEECNAME        VARCHAR(50),
   EMPLOYEEENAME        VARCHAR(50),
   EMPLOYEELEVEL        VARCHAR(50),
   SEX                  VARCHAR(1),
   PROFESSIONALTITLE    VARCHAR(50),
   IDTYPE               VARCHAR(50),
   IDNUMBER             VARCHAR(50),
   NATION               VARCHAR(50),
   HEIGHT               VARCHAR(50),
   BANKID               VARCHAR(50),
   BANKCARDNUMBER       VARCHAR(50),
   RESUMEID             VARCHAR(50),
   PROVINCE             VARCHAR(100),
   BLOODTYPE            VARCHAR(50),
   MARRIAGE             VARCHAR(50),
   HASCHILDREN          VARCHAR(50),
   POLITICS             VARCHAR(100),
   REGRESIDENCE         VARCHAR(200),
   REGRESIDENCETYPE     VARCHAR(50),
   RESIDENCETYPE        VARCHAR(50),
   EMAIL                VARCHAR(50),
   MOBILE               VARCHAR(50),
   SECONDLANGUAGE       VARCHAR(50),
   SECONDLANGUAGEDEGREE VARCHAR(50),
   OFFICEPHONE          VARCHAR(50),
   CURRENTADDRESS       VARCHAR(100),
   URGENCYPERSON        VARCHAR(50),
   URGENCYCONTACT       VARCHAR(50),
   FAMILYADDRESS        VARCHAR(100),
   FAMILYZIPCODE        VARCHAR(100),
   FAMILYPHONE          VARCHAR(50),
   FINGERPRINTID        VARCHAR(50),
   PHOTO                LONGBLOB,
   TOPEDUCATION         VARCHAR(100),
   BIRTHDAY             DATETIME,
   EMPLOYEESTATE        VARCHAR(50),
   WORKINGAGE           NUMERIC(8,0),
   EDITSTATE            VARCHAR(1),
   REMARK               VARCHAR(2000),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME,
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME,
   EMPLOYEETYPE         VARCHAR(1),
   OTHERCOMMUNICATE     VARCHAR(500),
   INTERESTCONTENT      VARCHAR(500),
   SOCIALSERVICEYEAR    VARCHAR(50)
);

ALTER TABLE T_HR_EMPLOYEE_LIUHH COMMENT 'T_HR_EMPLOYEE_LIUHH';

/*==============================================================*/
/* Table: T_HR_EDUCATEHISTORY                                   */
/*==============================================================*/
CREATE TABLE T_HR_EDUCATEHISTORY
(
   EDUCATEHISTORYID     VARCHAR(50) NOT NULL COMMENT '教育培训ID',
   RESUMEID             VARCHAR(50) COMMENT '简历id',
   SCHOONAME            VARCHAR(50) COMMENT '学校名称',
   SPECIALTY            VARCHAR(50) COMMENT '专业',
   MAJOR                VARCHAR(50) COMMENT '主修课程',
   STARTDATE            VARCHAR(50) COMMENT '开始日期',
   ENDDATE              VARCHAR(50) COMMENT '结束日期',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   EDUCATIONHISTORY     VARCHAR(50) COMMENT '学历',
   EDUCATIONPROPERTIE   VARCHAR(50) COMMENT '教育性质',
   PRIMARY KEY (EDUCATEHISTORYID)
);

ALTER TABLE T_HR_EDUCATEHISTORY COMMENT '教育培训记录';

/*==============================================================*/
/* Table: T_HR_EMPLOYEEABNORMRECORD                             */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEEABNORMRECORD
(
   ABNORMRECORDID       VARCHAR(50) NOT NULL COMMENT '异常记录子表ID',
   ATTENDANCERECORDID   VARCHAR(50) COMMENT '考勤记录编号',
   ABNORMALDATE         DATETIME COMMENT '异常日期',
   ABNORMCATEGORY       VARCHAR(50) COMMENT '缺勤',
   ATTENDPERIOD         VARCHAR(1) COMMENT '上午,中午,下午,晚上',
   ABNORMALTIME         NUMERIC(8,0) COMMENT '按小时算',
   SINGINSTATE          VARCHAR(1) COMMENT '签卡状态：1：未签卡，2：已签卡，3：签卡中',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   PRIMARY KEY (ABNORMRECORDID)
);

ALTER TABLE T_HR_EMPLOYEEABNORMRECORD COMMENT '员工异常记录子表';

/*==============================================================*/
/* Index: CORDOWID                                              */
/*==============================================================*/
CREATE INDEX CORDOWID ON T_HR_EMPLOYEEABNORMRECORD
(
   OWNERID
);

/*==============================================================*/
/* Index: IDX_EMPLOT_ATIDGORYRIOD                               */
/*==============================================================*/
CREATE INDEX IDX_EMPLOT_ATIDGORYRIOD ON T_HR_EMPLOYEEABNORMRECORD
(
   ATTENDANCERECORDID,
   ABNORMCATEGORY,
   ATTENDPERIOD
);

/*==============================================================*/
/* Table: T_HR_EMPLOYEEADDSUM                                   */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEEADDSUM
(
   ADDSUMID             VARCHAR(50) NOT NULL COMMENT '加扣款ID',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   PROJECTNAME          VARCHAR(50) COMMENT '项目类型(1.员工加扣款2.员工代扣款)',
   PROJECTCODE          VARCHAR(50) COMMENT '项目编码',
   PROJECTMONEY         NUMERIC(8,0) COMMENT '项目金额',
   SYSTEMTYPE           VARCHAR(50) COMMENT '来源系统',
   DEALYEAR             VARCHAR(50) COMMENT '处理年份',
   DEALMONTH            VARCHAR(50) COMMENT '处理月份',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   MONTHLYBATCHID       VARCHAR(50) COMMENT '月度批量结算ID',
   PRIMARY KEY (ADDSUMID)
);

ALTER TABLE T_HR_EMPLOYEEADDSUM COMMENT '提供给外部系统处理员工加扣款项';

/*==============================================================*/
/* Index: IDX_SUMCOID                                           */
/*==============================================================*/
CREATE INDEX IDX_SUMCOID ON T_HR_EMPLOYEEADDSUM
(
   OWNERCOMPANYID,
   EMPLOYEEID
);

/*==============================================================*/
/* Index: INX_HRESUMEID                                         */
/*==============================================================*/
CREATE INDEX INX_HRESUMEID ON T_HR_EMPLOYEEADDSUM
(
   EMPLOYEEID
);

/*==============================================================*/
/* Table: T_HR_EMPLOYEEADDSUMBATCH                              */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEEADDSUMBATCH
(
   MONTHLYBATCHID       VARCHAR(50) NOT NULL COMMENT '月度批量结算ID',
   BALANCEYEAR          NUMERIC(8,0) COMMENT '结算年份',
   BALANCEMONTH         NUMERIC(8,0) COMMENT '结算月份',
   BALANCEDATE          DATETIME COMMENT '结算日期',
   BALANCEOBJECTTYPE    VARCHAR(1) COMMENT '结算对象类型',
   BALANCEOBJECTID      VARCHAR(50) COMMENT '结算对象Id',
   BALANCEOBJECTNAME    VARCHAR(500) COMMENT '结算对象名',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   REMARK               VARCHAR(2000) COMMENT '备注',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (MONTHLYBATCHID)
);

ALTER TABLE T_HR_EMPLOYEEADDSUMBATCH COMMENT '员工加扣款批量审核表';

/*==============================================================*/
/* Table: T_HR_EMPLOYEECANCELLEAVE                              */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEECANCELLEAVE
(
   CANCELLEAVEID        VARCHAR(50) NOT NULL COMMENT '销假记录ID',
   LEAVERECORDID        VARCHAR(50) COMMENT '请假记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   STARTDATETIME        DATETIME COMMENT '销假开始时间',
   ENDDATETIME          DATETIME COMMENT '销假结束时间',
   LEAVEDAYS            NUMERIC(8,0) COMMENT '销假天数',
   LEAVEHOURS           NUMERIC(8,0) COMMENT '销假时长',
   TOTALHOURS           NUMERIC(8,0) COMMENT '销假合计时长',
   REASON               VARCHAR(50) COMMENT '销假原因',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (CANCELLEAVEID)
);

ALTER TABLE T_HR_EMPLOYEECANCELLEAVE COMMENT '员工销假申请';

/*==============================================================*/
/* Table: T_HR_EMPLOYEECHECK                                    */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEECHECK
(
   BEREGULARID          VARCHAR(50) NOT NULL COMMENT '转正表ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   PROBATIONPERIOD      NUMERIC(38,0) COMMENT '试用期',
   CHECKDATE            DATETIME COMMENT '审核时间',
   CHECKRESULT          VARCHAR(50) COMMENT '审核结果',
   CHECKCONTENT         TEXT COMMENT '考核表',
   SELFCHECKRESULT      VARCHAR(50) COMMENT '自评结果',
   CHECKUSER            VARCHAR(50) COMMENT '审核人',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   BEREGULARDATE        DATETIME COMMENT '转正时间',
   CHECKSTATE           VARCHAR(50) COMMENT '入职审核状态',
   ONDUTYDATE           DATETIME COMMENT '到岗日期',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   REPORTDATE           DATETIME COMMENT '入职时间',
   REMARK               VARCHAR(2000) COMMENT '备注',
   PRIMARY KEY (BEREGULARID)
);

ALTER TABLE T_HR_EMPLOYEECHECK COMMENT '员工转正表#';

/*==============================================================*/
/* Table: T_HR_EMPLOYEECLOCKINRECORD                            */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEECLOCKINRECORD
(
   CLOCKINRECORDID      VARCHAR(50) NOT NULL COMMENT '打卡记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   CLOCKID              VARCHAR(2000) COMMENT '卡钟ID',
   FINGERPRINTID        VARCHAR(50) COMMENT '指纹编码',
   PUNCHDATE            DATETIME COMMENT '打卡日期',
   PUNCHTIME            VARCHAR(100) COMMENT '打卡时间',
   VERIFYCODE           NUMERIC(8,0) COMMENT '验证方式:0-指纹；1-输号码；2-密码',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (CLOCKINRECORDID)
);

ALTER TABLE T_HR_EMPLOYEECLOCKINRECORD COMMENT '从考勤机导入';

/*==============================================================*/
/* Index: IDX_CORDOWNERID                                       */
/*==============================================================*/
CREATE INDEX IDX_CORDOWNERID ON T_HR_EMPLOYEECLOCKINRECORD
(
   OWNERID
);

/*==============================================================*/
/* Index: IDX_FINGERPRINTID                                     */
/*==============================================================*/
CREATE INDEX IDX_FINGERPRINTID ON T_HR_EMPLOYEECLOCKINRECORD
(
   FINGERPRINTID
);

/*==============================================================*/
/* Index: IDX_LOCKEMPLOYEEID                                    */
/*==============================================================*/
CREATE INDEX IDX_LOCKEMPLOYEEID ON T_HR_EMPLOYEECLOCKINRECORD
(
   EMPLOYEEID
);

/*==============================================================*/
/* Index: IDX_OWN_EMP_HRMEMP                                    */
/*==============================================================*/
CREATE INDEX IDX_OWN_EMP_HRMEMP ON T_HR_EMPLOYEECLOCKINRECORD
(
   OWNERCOMPANYID,
   EMPLOYEEID
);

/*==============================================================*/
/* Index: IDX_RECORDPUNCHDATE                                   */
/*==============================================================*/
CREATE INDEX IDX_RECORDPUNCHDATE ON T_HR_EMPLOYEECLOCKINRECORD
(
   PUNCHDATE
);

/*==============================================================*/
/* Table: T_HR_EMPLOYEECONTRACT                                 */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEECONTRACT
(
   EMPLOYEECONTACTID    VARCHAR(50) NOT NULL COMMENT '员工合同ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   CONTACTCODE          VARCHAR(50) COMMENT '合同编号',
   FROMDATE             DATETIME COMMENT '合同生效日期从',
   TODATE               VARCHAR(50) COMMENT '至',
   CONTACTPERIOD        NUMERIC(38,0) COMMENT '合同期限',
   ENDDATE              DATETIME COMMENT '终止日期',
   REASON               VARCHAR(50) COMMENT '终止原因',
   ATTACHMENT           LONGBLOB COMMENT '合同扫描件',
   ATTACHMENTPATH       VARCHAR(2000) COMMENT '合同扫描件地址',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   ISSPECIALCONTRACT    VARCHAR(50) COMMENT '是否特殊合同',
   ALARMDAY             VARCHAR(50) COMMENT '提前多少天提醒',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   NOENDDATE            VARCHAR(50) COMMENT '是否为无固定期限合同：0：不是，1：是',
   PRIMARY KEY (EMPLOYEECONTACTID)
);

ALTER TABLE T_HR_EMPLOYEECONTRACT COMMENT '员工劳动合同(未加权限)';

/*==============================================================*/
/* Table: T_HR_EMPLOYEEENTRY                                    */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEEENTRY
(
   EMPLOYEEENTRYID      VARCHAR(50) NOT NULL COMMENT '入职表ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   CHECKSTATE           VARCHAR(50) COMMENT '入职审核状态',
   ENTRYDATE            DATETIME COMMENT '入职时间',
   ONPOSTDATE           DATETIME COMMENT '到岗日期',
   PROBATIONPERIOD      NUMERIC(8,0) COMMENT '试用期',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   REMARK               VARCHAR(200) COMMENT '备注',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   EMPLOYEEPOSTID       VARCHAR(36) COMMENT '员工岗位ID',
   PRIMARY KEY (EMPLOYEEENTRYID)
);

ALTER TABLE T_HR_EMPLOYEEENTRY COMMENT '员工入职表#';

/*==============================================================*/
/* Table: T_HR_EMPLOYEEEVECTIONRECORD                           */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEEEVECTIONRECORD
(
   EVECTIONRECORDID     VARCHAR(50) NOT NULL COMMENT '出差记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   EVECTIONRECORDCATEGORY VARCHAR(50) COMMENT '出差类型',
   STARTTIME            VARCHAR(50) COMMENT '出差开始时间',
   ENDTIME              VARCHAR(50) COMMENT '出差结束时间',
   STARTDATE            DATETIME COMMENT '出差开始日期',
   ENDDATE              DATETIME COMMENT '出差结束日期',
   TOTALDAYS            NUMERIC(8,0) COMMENT '出差天数',
   DESTINATION          VARCHAR(100) COMMENT '出差目的地',
   EVECTIONREASON       VARCHAR(500) COMMENT '出差原因',
   REPLACEPEOPLE        VARCHAR(50) COMMENT '工作承接人',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   SUBSIDYTYPE          VARCHAR(50) COMMENT '0按次，1按天',
   SUBSIDYVALUE         NUMERIC(8,0) COMMENT '补助金额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (EVECTIONRECORDID)
);

ALTER TABLE T_HR_EMPLOYEEEVECTIONRECORD COMMENT '员工出差记录';

/*==============================================================*/
/* Table: T_HR_EMPLOYEEEVECTIONREPORT                           */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEEEVECTIONREPORT
(
   EVECTIONREPORTID     VARCHAR(50) NOT NULL COMMENT '出差报告ID',
   EVECTIONRECORDID     VARCHAR(50) COMMENT '出差记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   EVECTIONRECORDCATEGORY VARCHAR(50) COMMENT '出差类型',
   STARTTIME            VARCHAR(50) COMMENT '出差开始时间',
   ENDTIME              VARCHAR(50) COMMENT '出差结束时间',
   STARTDATE            DATETIME COMMENT '出差开始日期',
   ENDDATE              DATETIME COMMENT '出差结束日期',
   TOTALDAYS            NUMERIC(8,0) COMMENT '出差天数',
   DESTINATION          VARCHAR(100) COMMENT '出差目的地',
   EVECTIONREASON       VARCHAR(500) COMMENT '出差原因',
   REPLACEPEOPLE        VARCHAR(50) COMMENT '工作承接人',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   SUBSIDYTYPE          VARCHAR(50) COMMENT '0按次，1按天',
   SUBSIDYVALUE         NUMERIC(8,0) COMMENT '补助金额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (EVECTIONREPORTID)
);

ALTER TABLE T_HR_EMPLOYEEEVECTIONREPORT COMMENT '员工出差报告';

/*==============================================================*/
/* Table: T_HR_EMPLOYEEINSURANCE                                */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEEINSURANCE
(
   EMPLOYINSURANCEID    VARCHAR(50) NOT NULL COMMENT '保险记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   INSURANCECATEGORY    VARCHAR(50) COMMENT '保险类型',
   INSURANCENAME        VARCHAR(50) COMMENT '保险名称',
   INSURANCECOMPANY     VARCHAR(50) COMMENT '保险公司名称',
   INSURANCECOST        NUMERIC(8,0) COMMENT '保险金额',
   CONTRACTCODE         VARCHAR(50) COMMENT '保险合同号',
   STARTDATE            VARCHAR(50) COMMENT '开始缴纳时间',
   LASTDATE             VARCHAR(50) COMMENT '最后一次缴纳时间',
   PERIOD               VARCHAR(50) COMMENT '缴纳周期',
   ALARMDAY             VARCHAR(50) COMMENT '提前多少天提醒',
   INSURANCEPAY         VARCHAR(50) COMMENT '缴纳金额',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (EMPLOYINSURANCEID)
);

ALTER TABLE T_HR_EMPLOYEEINSURANCE COMMENT '员工保险记录(未加权限)';

/*==============================================================*/
/* Table: T_HR_EMPLOYEELEAVERECORD                              */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEELEAVERECORD
(
   LEAVERECORDID        VARCHAR(50) NOT NULL COMMENT '请假记录ID',
   LEAVETYPESETID       VARCHAR(50) COMMENT '请假类型ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   STARTDATETIME        DATETIME COMMENT '请假开始时间',
   ENDDATETIME          DATETIME COMMENT '请假结束时间',
   LEAVEDAYS            NUMERIC(8,0) COMMENT '请假天数',
   LEAVEHOURS           NUMERIC(8,0) COMMENT '请假时长',
   TOTALHOURS           NUMERIC(8,0) COMMENT '请假合计时长',
   REASON               VARCHAR(2000) COMMENT '请假原因',
   ATTACHMENTPATH       VARCHAR(50) COMMENT '附件路径',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   FINEDAYS             NUMERIC(8,0) DEFAULT 0,
   FINEHOURS            NUMERIC(8,0) DEFAULT 0,
   USEABLEHOURS         NUMERIC(8,0) DEFAULT 0,
   USEABLEDAYS          NUMERIC(8,0) DEFAULT 0,
   PRIMARY KEY (LEAVERECORDID)
);

ALTER TABLE T_HR_EMPLOYEELEAVERECORD COMMENT '员工请假记录';

/*==============================================================*/
/* Table: T_HR_EMPLOYEELEVELDAYCOUNT                            */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEELEVELDAYCOUNT
(
   RECORDID             VARCHAR(50) NOT NULL COMMENT '记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   VACATIONTYPE         VARCHAR(50) COMMENT '休假类型',
   DAYS                 NUMERIC(8,0) COMMENT '可休天数',
   EFFICDATE            DATETIME COMMENT '生效日期',
   TERMINATEDATE        DATETIME COMMENT '终止日期',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   LEAVETYPESETID       VARCHAR(50) COMMENT '请假类型ID',
   STATUS               NUMERIC(8,0) DEFAULT 1,
   LEFTHOURS            NUMERIC(8,0) DEFAULT 0,
   HOURS                NUMERIC(8,0) DEFAULT 0,
   PRIMARY KEY (RECORDID)
);

ALTER TABLE T_HR_EMPLOYEELEVELDAYCOUNT COMMENT '员工可休假表';

/*==============================================================*/
/* Index: IDX_EIDPID                                            */
/*==============================================================*/
CREATE INDEX IDX_EIDPID ON T_HR_EMPLOYEELEVELDAYCOUNT
(
   EMPLOYEEID,
   OWNERCOMPANYID
);

/*==============================================================*/
/* Table: T_HR_EMPLOYEELEVELDAYDETAILS                          */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEELEVELDAYDETAILS
(
   LEVELDAYDETAILSID    VARCHAR(50) NOT NULL COMMENT '可休假明细ID',
   RECORDID             VARCHAR(50) COMMENT '记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   VACATIONTYPE         VARCHAR(1) COMMENT '休假类型',
   DAYS                 NUMERIC(8,0) COMMENT '增减天数',
   EFFICDATE            DATETIME COMMENT '变动日期',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (LEVELDAYDETAILSID)
);

ALTER TABLE T_HR_EMPLOYEELEVELDAYDETAILS COMMENT '员工可休假明细';

/*==============================================================*/
/* Table: T_HR_EMPLOYEELOGINRECORD                              */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEELOGINRECORD
(
   LOGINRECORDID        VARCHAR(50) NOT NULL COMMENT '登录系统记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   USERNAME             VARCHAR(50) NOT NULL COMMENT '员工系统帐号',
   LOGINDATE            VARCHAR(50) COMMENT '登录日期',
   LOGINTIME            VARCHAR(50) COMMENT '登录时间',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   PRIMARY KEY (LOGINRECORDID)
);

ALTER TABLE T_HR_EMPLOYEELOGINRECORD COMMENT '员工登录系统记录表';

/*==============================================================*/
/* Table: T_HR_EMPLOYEEOVERTIMEDETAILRD                         */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEEOVERTIMEDETAILRD
(
   OVERTIMEDETAILRDID   VARCHAR(50) NOT NULL COMMENT '加班明细记录ID',
   OVERTIMERECORDID     VARCHAR(50) COMMENT '加班记录ID',
   STARTDATETIME        DATETIME COMMENT '加班开始时间',
   ENDDATETIME          DATETIME COMMENT '加班结束时间',
   OVERTIMEHOURS        VARCHAR(50) COMMENT '加班时长',
   PAYCATEGORY          VARCHAR(50) COMMENT '加班报酬方式：0 调休方式、1 加班工资方式、2 先调休再付工资方式、3 无报酬方式；',
   REMARK               VARCHAR(500) COMMENT '备注',
   PRIMARY KEY (OVERTIMEDETAILRDID)
);

ALTER TABLE T_HR_EMPLOYEEOVERTIMEDETAILRD COMMENT '员工加班记录明细';

/*==============================================================*/
/* Table: T_HR_EMPLOYEEOVERTIMERECORD                           */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEEOVERTIMERECORD
(
   OVERTIMERECORDID     VARCHAR(50) NOT NULL COMMENT '加班记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   OVERTIMECATE         VARCHAR(50) COMMENT '加班生效方式
            0 审核通过的加班申请；1 超过工作时间外自动累计；2 仅节假日累计；',
   STARTDATE            DATETIME COMMENT '加班开始日期',
   STARTDATETIME        VARCHAR(50) COMMENT '加班开始时间',
   ENDDATE              DATETIME COMMENT '加班结束日期',
   ENDDATETIME          VARCHAR(50) COMMENT '加班结束时间',
   OVERTIMEHOURS        NUMERIC(8,0) COMMENT '加班时长',
   PAYCATEGORY          VARCHAR(50) COMMENT '加班报酬方式：0 调休方式、1 加班工资方式、2 先调休再付工资方式、3 无报酬方式；',
   ISCONVERTLEVEDAY     VARCHAR(1) COMMENT '是否已兑换调休',
   BEGINCARDTIME        VARCHAR(50) COMMENT '上班刷卡时间',
   BEGINCARDTYPE        VARCHAR(1) COMMENT '0,正常刷卡,1 签卡',
   BEGINCARDSTATE       VARCHAR(50) COMMENT '上班状态',
   ENDCARDTIME          VARCHAR(50) COMMENT '下班刷卡时间',
   ENDCARDTYPE          VARCHAR(1) COMMENT '0,正常刷卡,1 签卡',
   ENDCARDSTATE         VARCHAR(50) COMMENT '下班状态',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   STATUS               NUMERIC(8,0) DEFAULT 0 COMMENT '0，有效；1，过期',
   EFFECTIVEDATE        DATETIME  COMMENT '生效日期',
   EXPIREDATE           DATETIME  COMMENT '过期时间',
   LEFTHOURS            NUMERIC(8,2) DEFAULT 0,
   PRIMARY KEY (OVERTIMERECORDID)
);

ALTER TABLE T_HR_EMPLOYEEOVERTIMERECORD COMMENT '员工加班记录';

/*==============================================================*/
/* Table: T_HR_EMPLOYEEPOSTCHANGE                               */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEEPOSTCHANGE
(
   POSTCHANGEID         VARCHAR(50) NOT NULL COMMENT '异动记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   POSTCHANGCATEGORY    VARCHAR(50) COMMENT '异动类型',
   POSTCHANGREASON      VARCHAR(2000) COMMENT '异动原因',
   FROMCOMPANYID        VARCHAR(50) COMMENT '异动前机构ID',
   TOCOMPANYID          VARCHAR(50) COMMENT '异动后机构ID',
   FROMDEPARTMENTID     VARCHAR(50) COMMENT '异动前部门ID',
   TODEPARTMENTID       VARCHAR(50) COMMENT '异动后部门ID',
   FROMPOSTID           VARCHAR(50) COMMENT '异动前岗位ID',
   TOPOSTID             VARCHAR(50) COMMENT '异动后岗位ID',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   CHANGEDATE           VARCHAR(50) COMMENT '异动日期',
   FROMPOSTLEVEL        NUMERIC(8,0) COMMENT '异动前岗位等级',
   FROMSALARYLEVEL      NUMERIC(8,0) COMMENT '异动前薪资等级',
   ISAGENCY             VARCHAR(50) COMMENT '是否代理岗位',
   TOPOSTLEVEL          NUMERIC(8,0) COMMENT '异动后岗位等级',
   TOSALARYLEVEL        NUMERIC(8,0) COMMENT '异动后薪资等级',
   EMPLOYEEPOSTID       VARCHAR(50) COMMENT '异动后员工岗位ID',
   ENDDATE              DATETIME COMMENT '结束日期',
   ENDREASON            VARCHAR(100) COMMENT '结束原因',
   PRIMARY KEY (POSTCHANGEID)
);

ALTER TABLE T_HR_EMPLOYEEPOSTCHANGE COMMENT '员工异动记录#';

/*==============================================================*/
/* Table: T_HR_EMPLOYEESALARYRECORD                             */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEESALARYRECORD
(
   EMPLOYEESALARYRECORDID VARCHAR(50) NOT NULL COMMENT '薪资记录ID',
   SALARYSTANDARDID     VARCHAR(50) COMMENT '薪资标准ID',
   EMPLOYEEID           VARCHAR(50) NOT NULL COMMENT '员工ID',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   SALARYMONTH          VARCHAR(50) COMMENT '所属月份',
   SALARYYEAR           VARCHAR(50) COMMENT '所属年份',
   ATTENDANCEUNUSUALDEDUCT NUMERIC(8,0) COMMENT '考勤异常扣款',
   ATTENDANCEUNUSUALTIME VARCHAR(50) COMMENT '考勤异常次数',
   ATTENDANCEUNUSUALTIMES VARCHAR(50) COMMENT '考勤异常时长',
   PAIDDATE             DATETIME COMMENT '发薪日期',
   PAIDLATEDATE         DATETIME COMMENT '缓发日期',
   OVERTIMETIMES        VARCHAR(50) COMMENT '加班时长',
   OVERTIMESUM          NUMERIC(8,0) COMMENT '加班费',
   ABSENTTIMES          VARCHAR(50) COMMENT '旷工时长',
   ABSENTDEDUCT         NUMERIC(8,0) COMMENT '旷工扣款',
   LEAVETIME            VARCHAR(50) COMMENT '请假时长',
   LEAVEDEDUCT          NUMERIC(8,0) COMMENT '请假扣款',
   EVECTIONTIMES        NUMERIC(8,0) COMMENT '出差时长',
   EVECTIONSUBSIDY      NUMERIC(8,0) COMMENT '出差补助',
   PAIDBY               VARCHAR(50) COMMENT '发薪经手人',
   PAIDTYPE             VARCHAR(50) COMMENT '0银行，现金，从方案中默认，但可以修改',
   ACTUALLYPAY          VARCHAR(2000) COMMENT '实发工资',
   BASICSALARY          NUMERIC(8,0) COMMENT '基本工资',
   POSTSALARY           NUMERIC(8,0) COMMENT '岗位工资',
   SECURITYALLOWANCE    NUMERIC(8,0) COMMENT '保密津贴',
   HOUSINGALLOWANCE     NUMERIC(8,0) COMMENT '住房津贴',
   AREADIFALLOWANCE     NUMERIC(8,0) COMMENT '地区差异补贴',
   FOODALLOWANCE        NUMERIC(8,0) COMMENT '餐费补贴',
   FIXEDINCOMESUM       NUMERIC(8,0) COMMENT '固定收入合计',
   ABSENCEDAYS          NUMERIC(8,0) COMMENT '缺勤天数',
   VACATIONDEDUCT       NUMERIC(8,0) COMMENT '假期其他扣款',
   WORKINGSALARY        NUMERIC(8,0) COMMENT '出勤工资',
   OTHERADDDEDUCT       NUMERIC(8,0) COMMENT '其他加扣款',
   SUBTOTAL             NUMERIC(8,0) COMMENT '应发小计',
   HOUSINGALLOWANCEDEDUCT NUMERIC(8,0) COMMENT '住房津贴扣款',
   PERSONALSICOST       NUMERIC(8,0) COMMENT '个人社保负担',
   PRETAXSUBTOTAL       NUMERIC(8,0) COMMENT '税前应发合计',
   BALANCE              NUMERIC(8,0) COMMENT '差额',
   PERSONALINCOMETAX    NUMERIC(8,0) COMMENT '个人所得税',
   OTHERSUBJOIN         NUMERIC(8,0) COMMENT '其它代扣款',
   OFFENCEDEDUCT        NUMERIC(8,0) COMMENT '违纪扣款',
   MANTISSADEDUCT       NUMERIC(8,0) COMMENT '尾数扣款',
   DEDUCTTOTAL          NUMERIC(8,0) COMMENT '扣款合计',
   PERFORMANCESUM       NUMERIC(8,0) COMMENT '绩效奖金额',
   CUSTOMERSUM          NUMERIC(8,0) COMMENT '自定义项金额',
   CONFIRM              VARCHAR(50) COMMENT '领款确认',
   CONFIRMDATE          DATETIME COMMENT '领款确认日期',
   DRAWMONEYREMARK      VARCHAR(50) COMMENT '领款备注',
   LOANDEDUCT           NUMERIC(8,0) COMMENT '借款抵扣',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PAYCONFIRM           VARCHAR(1) COMMENT '发放标志',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   MONTHLYBATCHID       VARCHAR(50) COMMENT '月度批量结算ID',
   PRIMARY KEY (EMPLOYEESALARYRECORDID)
);

ALTER TABLE T_HR_EMPLOYEESALARYRECORD COMMENT '薪资记录';

/*==============================================================*/
/* Table: T_HR_EMPLOYEESALARYRECORDITEM                         */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEESALARYRECORDITEM
(
   SALARYRECORDITEMID   VARCHAR(50) NOT NULL COMMENT '薪资项ID',
   EMPLOYEESALARYRECORDID VARCHAR(50) COMMENT '薪资记录ID',
   SALARYARCHIVEID      VARCHAR(50) COMMENT '薪资档案ID',
   SALARYSTANDARDID     VARCHAR(50) COMMENT '薪资标准ID',
   SALARYITEMID         VARCHAR(50) COMMENT '计算项ID',
   CALCULATEFORMULA     VARCHAR(2000) COMMENT '计算公式',
   SUM                  VARCHAR(2000) COMMENT '金额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   ORDERNUMBER          NUMERIC(8,0) COMMENT '排序号',
   PRIMARY KEY (SALARYRECORDITEMID)
);

ALTER TABLE T_HR_EMPLOYEESALARYRECORDITEM COMMENT '薪资记录薪资项(#)';

/*==============================================================*/
/* Table: T_HR_EMPLOYEESIGNINDETAIL                             */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEESIGNINDETAIL
(
   SIGNINDETAILID       VARCHAR(50) NOT NULL COMMENT '异常记录子表ID',
   SIGNINID             VARCHAR(50) COMMENT '签卡记录ID',
   ABNORMRECORDID       VARCHAR(50) COMMENT '异常记录子表ID2',
   ABNORMALDATE         DATETIME COMMENT '异常日期',
   ABNORMCATEGORY       VARCHAR(50) COMMENT '缺勤',
   ATTENDPERIOD         VARCHAR(1) COMMENT '上午,中午,下午,晚上',
   ABNORMALTIME         NUMERIC(8,0) COMMENT '按小时算',
   REASONCATEGORY       VARCHAR(1) COMMENT '漏打卡,因工外出,等',
   DETAILREASON         VARCHAR(2000) COMMENT '详细原因',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   PRIMARY KEY (SIGNINDETAILID)
);

ALTER TABLE T_HR_EMPLOYEESIGNINDETAIL COMMENT '员工签卡记录子表';

/*==============================================================*/
/* Index: IDX_TAILOWNERID                                       */
/*==============================================================*/
CREATE INDEX IDX_TAILOWNERID ON T_HR_EMPLOYEESIGNINDETAIL
(
   OWNERID
);

/*==============================================================*/
/* Table: T_HR_EMPLOYEESIGNINRECORD                             */
/*==============================================================*/
CREATE TABLE T_HR_EMPLOYEESIGNINRECORD
(
   SIGNINID             VARCHAR(50) NOT NULL COMMENT '签卡记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   SIGNINTIME           DATETIME COMMENT '签卡日期',
   CHECKSTATE           VARCHAR(1) COMMENT '0,漏打,1因公外出,2机械故障',
   SIGNINCATEGORY       VARCHAR(50) COMMENT '签卡类型',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (SIGNINID)
);

ALTER TABLE T_HR_EMPLOYEESIGNINRECORD COMMENT '员工签卡记录表';

/*==============================================================*/
/* Table: T_HR_EXPERIENCE                                       */
/*==============================================================*/
CREATE TABLE T_HR_EXPERIENCE
(
   EXPERIENCEID         VARCHAR(50) NOT NULL COMMENT '工作经历ID',
   RESUMEID             VARCHAR(50) COMMENT '简历id',
   COMPANYNAME          VARCHAR(50) COMMENT '公司名称',
   POST                 VARCHAR(50) COMMENT '工作岗位',
   SALARY               VARCHAR(50) COMMENT '月薪',
   STARTDATE            VARCHAR(50) COMMENT '开始日期',
   ENDDATE              VARCHAR(50) COMMENT '结束日期',
   JOBDESCRIPTION       VARCHAR(2000) COMMENT '工作描述',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (EXPERIENCEID)
);

ALTER TABLE T_HR_EXPERIENCE COMMENT '工作经历';

/*==============================================================*/
/* Table: T_HR_FAMILYMEMBER                                     */
/*==============================================================*/
CREATE TABLE T_HR_FAMILYMEMBER
(
   FAMILYMEMBERID       VARCHAR(50) NOT NULL COMMENT '成员信息ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   NAME                 VARCHAR(50) COMMENT '姓名',
   AGE                  VARCHAR(50) COMMENT '年龄',
   SEX                  VARCHAR(1) COMMENT '性别',
   RELATIONSHIP         VARCHAR(50) COMMENT '与员工关系',
   CONTACT              VARCHAR(50) COMMENT '联系方式',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (FAMILYMEMBERID)
);

ALTER TABLE T_HR_FAMILYMEMBER COMMENT '员工家庭成员信息表';

/*==============================================================*/
/* Table: T_HR_FREELEAVEDAYSET                                  */
/*==============================================================*/
CREATE TABLE T_HR_FREELEAVEDAYSET
(
   FREELEAVEDAYSETID    VARCHAR(50) NOT NULL COMMENT '带薪假设置ID',
   LEAVETYPESETID       VARCHAR(50) COMMENT '请假类型ID',
   MINIMONTH            NUMERIC(8,0) COMMENT '入职最小月份',
   MAXMONTH             NUMERIC(8,0) COMMENT '入职最大月份',
   LEAVEDAYS            NUMERIC(8,0) COMMENT '可休假数',
   ISPERFECTATTENDANCEFACTOR VARCHAR(50) COMMENT '是否扣全勤',
   OFFESTTYPE           VARCHAR(1) COMMENT '1、一次性休完，此时不得用于冲减病事假；2、分N次修完，每冲减一次病事假算一次；3、无限制，可用于冲减病事假；',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (FREELEAVEDAYSETID)
);

ALTER TABLE T_HR_FREELEAVEDAYSET COMMENT '记录员工可带薪休假天数，假期类型可以是年假，也可以是别的假日';

/*==============================================================*/
/* Table: T_HR_IMPORTSETDETAIL                                  */
/*==============================================================*/
CREATE TABLE T_HR_IMPORTSETDETAIL
(
   DETAILID             VARCHAR(50) NOT NULL COMMENT '导入设置明细表ID',
   MASTERID             VARCHAR(50) COMMENT '导入设置主表ID',
   ENTITYCOLUMNNAME     VARCHAR(100) COMMENT '实体字段名',
   ENTITYCOLUMNCODE     VARCHAR(100) COMMENT '实体字段编码',
   EXECELCOLUMN         VARCHAR(50) COMMENT '对应Execll列',
   EXECELROW            NUMERIC(8,0) COMMENT '对应Execll行',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (DETAILID)
);

ALTER TABLE T_HR_IMPORTSETDETAIL COMMENT '导入设置明细表';

/*==============================================================*/
/* Table: T_HR_IMPORTSETMASTER                                  */
/*==============================================================*/
CREATE TABLE T_HR_IMPORTSETMASTER
(
   MASTERID             VARCHAR(50) NOT NULL COMMENT '导入设置主表ID',
   CITY                 VARCHAR(10) COMMENT '城市',
   ENTITYNAME           VARCHAR(100) COMMENT '实体名称',
   ENTITYCODE           VARCHAR(100) COMMENT '实体编码',
   STARTCOLUMN          VARCHAR(50) COMMENT '开始读取列',
   STARTROW             NUMERIC(8,0) COMMENT '开始读取行',
   ENDROW               NUMERIC(8,0) COMMENT '结束读取行',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (MASTERID)
);

ALTER TABLE T_HR_IMPORTSETMASTER COMMENT '导入设置主表';

/*==============================================================*/
/* Table: T_HR_KPIPOINT                                         */
/*==============================================================*/
CREATE TABLE T_HR_KPIPOINT
(
   KPIPOINTID           VARCHAR(50) NOT NULL COMMENT 'KPI点ID',
   SCORETYPEID          VARCHAR(50) COMMENT '评分方式ID',
   KPIPOINTNAME         VARCHAR(50) COMMENT 'KPI名称',
   SYSTEMID             VARCHAR(50) COMMENT '系统ID',
   BUSINESSID           VARCHAR(50) COMMENT '业务ID',
   FLOWID               VARCHAR(50) COMMENT '流程ID',
   STEPID               VARCHAR(50) COMMENT '步骤ID',
   KPIPOINTREMARK       VARCHAR(2000) COMMENT '说明',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   SUMTYPE              VARCHAR(50) COMMENT '汇总类别',
   PRIMARY KEY (KPIPOINTID)
);

ALTER TABLE T_HR_KPIPOINT COMMENT 'KPI点';

/*==============================================================*/
/* Table: T_HR_KPIPOINTDEFINE                                   */
/*==============================================================*/
CREATE TABLE T_HR_KPIPOINTDEFINE
(
   KPIPOINTID           VARCHAR(50) NOT NULL,
   CHECKCATEGORYID      VARCHAR(50),
   PERFORMANCEGOAL      VARCHAR(2000),
   CHECKMODELID         VARCHAR(50),
   ISFLOWSTEPCHECK      VARCHAR(1),
   FLOWID               VARCHAR(50),
   FLOWNAME             VARCHAR(50),
   FLOWSTEPID           VARCHAR(50),
   FLOWSTEPNAME         VARCHAR(50),
   DEALTIME             NUMERIC(8,0) COMMENT '系统定义的有效处理时间，按秒算',
   SYSTEMGRADEWEIGHING  NUMERIC(8,0) COMMENT '系统打分权重：系统自动计算',
   PERSONGRADEWEIGHING  NUMERIC(8,0),
   SPOTGRADEWEIGHING    NUMERIC(8,0),
   SPOTCHECKGROUPID     VARCHAR(50),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   REMARK               VARCHAR(2000),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME,
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (KPIPOINTID)
);

ALTER TABLE T_HR_KPIPOINTDEFINE COMMENT '考核项目点设置';

/*==============================================================*/
/* Table: T_HR_KPIRECORDCOMPLAIN                                */
/*==============================================================*/
CREATE TABLE T_HR_KPIRECORDCOMPLAIN
(
   COMPLAINID           VARCHAR(50) NOT NULL COMMENT '申诉记录ID',
   KPIRECORDID          VARCHAR(50) COMMENT 'KPI记录ID',
   COMPLAINANTID        VARCHAR(50) COMMENT '申诉人员ID',
   REVIEWERID           VARCHAR(50) COMMENT '审核人员ID',
   COMPLAINREMARK       VARCHAR(500) COMMENT '申诉内容',
   COMPLAINDATE         DATETIME COMMENT '申诉时间',
   CHECKSTATE           VARCHAR(1) COMMENT '0.不同意；1.同意；2.待审。',
   INITIALSCORE         NUMERIC(8,0) COMMENT '原始得分',
   REVIEWSCORE          NUMERIC(8,0) COMMENT '审核得分',
   REVIEWREMARK         VARCHAR(500) COMMENT '审核评语',
   REVIEWDATE           DATETIME COMMENT '审核时间',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (COMPLAINID)
);

ALTER TABLE T_HR_KPIRECORDCOMPLAIN COMMENT '申诉记录';

/*==============================================================*/
/* Table: T_HR_KPIREMIND                                        */
/*==============================================================*/
CREATE TABLE T_HR_KPIREMIND
(
   REMINDID             VARCHAR(50) NOT NULL COMMENT '评分提醒ID',
   SCORETYPEID          VARCHAR(50) COMMENT '评分方式ID',
   REMINDTYPE           VARCHAR(50) COMMENT '1.门户；2.邮件；3.短信；4.即时通讯软件(如：RTX)。可多个组合。',
   FORWARDHOURS         NUMERIC(8,0) COMMENT '提前小时数',
   PRIMARY KEY (REMINDID)
);

ALTER TABLE T_HR_KPIREMIND COMMENT '评分提醒';

/*==============================================================*/
/* Table: T_HR_KPIRECORD                                        */
/*==============================================================*/
CREATE TABLE T_HR_KPIRECORD
(
   KPIRECORDID          VARCHAR(50) NOT NULL COMMENT 'KPI记录ID',
   EMPLOYEEID           VARCHAR(50),
   EMPLOYEECODE         VARCHAR(50),
   EMPLOYEENAME         VARCHAR(50),
   KPIPOINTID           VARCHAR(50) COMMENT 'KPI点ID',
   APPRAISEEID          VARCHAR(50) COMMENT '被考核人员ID',
   APPRAISERID          VARCHAR(50) COMMENT '打分人员ID',
   RANDOMPERSONID       VARCHAR(50) COMMENT '抽查人员ID',
   BUSINESSCODE         VARCHAR(50) COMMENT '业务单号',
   FLOWRECORDCODE       VARCHAR(50) COMMENT '流程单号',
   STEPDETAILCODE       VARCHAR(50) COMMENT '步骤单号',
   SYSTEMSCORE          NUMERIC(8,0) COMMENT '系统评分',
   SYSTEMWEIGHT         NUMERIC(8,0) COMMENT '系统评分权重',
   MANUALSCORE          NUMERIC(8,0) COMMENT '手动评分',
   MANUALWEIGHT         NUMERIC(8,0) COMMENT '手动评分权重',
   RANDOMSCORE          NUMERIC(8,0) COMMENT '抽查打分',
   RANDOMWEIGHT         NUMERIC(8,0) COMMENT '抽查打分权重',
   SUMSCORE             NUMERIC(8,0) COMMENT '总分',
   T_H_SPOTCHECKERID    VARCHAR(50),
   FLOWID               VARCHAR(50),
   FLOWNAME             VARCHAR(50),
   FLOWSTEP             VARCHAR(50),
   CHECKDATE            DATETIME,
   SYSTEMGRADESCROE     NUMERIC(8,0),
   PERSONGRADESCROE     NUMERIC(8,0),
   PERSONID             VARCHAR(50),
   PERSONCOMMENT        VARCHAR(2000),
   SPOTGRADESCORE       NUMERIC(8,0),
   SPOTPERSONID         VARCHAR(50),
   SPOTPERSONCOMMENT    VARCHAR(2000),
   TOTALSCORE           VARCHAR(50),
   REMARK               VARCHAR(2000) COMMENT '评语',
   COMPLAINSTATUS       VARCHAR(1) COMMENT '0：没有申诉；1：正在申诉；2：已有申诉记录',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   KPICATEGORY          VARCHAR(1) COMMENT '绩效类型：0,流程，1任务',
   KPIDESCRIPTION       VARCHAR(200) COMMENT 'KPI点描述',
   FLOWDESCRIPTION      VARCHAR(200) COMMENT '流程/任务描述',
   PRIMARY KEY (KPIRECORDID)
);

ALTER TABLE T_HR_KPIRECORD COMMENT 'KPI明细记录';

/*==============================================================*/
/* Table: T_HR_KPIRECORDAPPEAL                                  */
/*==============================================================*/
CREATE TABLE T_HR_KPIRECORDAPPEAL
(
   KPIAPPEALID          VARCHAR(50) NOT NULL,
   KPIRECORDID          VARCHAR(50),
   APPEALREASON         VARCHAR(2000),
   CHECKSTATE           VARCHAR(1),
   EDITSTATE            VARCHAR(1),
   REMARK               VARCHAR(2000),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME ,
   PRIMARY KEY (KPIAPPEALID)
);

ALTER TABLE T_HR_KPIRECORDAPPEAL COMMENT 'KPI记录考核申诉';

/*==============================================================*/
/* Table: T_HR_KPIRECORDPERSONSUMMARY                           */
/*==============================================================*/
CREATE TABLE T_HR_KPIRECORDPERSONSUMMARY
(
   KPIRECORDSUMMARYID   VARCHAR(50) NOT NULL,
   EMPLOYEEID           VARCHAR(50),
   EMPLOYEECODE         VARCHAR(50),
   EMPLOYEENAME         VARCHAR(50),
   SYSTEMTYPE           VARCHAR(50),
   PERFORMYEAR          VARCHAR(50),
   PERFORMQUARTER       VARCHAR(50),
   PERFORMMOTH          VARCHAR(50),
   AVERAGE              NUMERIC(8,0),
   EXAMINEDATE          VARCHAR(50),
   EXAMINEPOINT         NUMERIC(8,0),
   GRADEPERSONID        VARCHAR(50),
   LEADCOMMENT          VARCHAR(2000),
   CHECKSTATE           VARCHAR(1),
   EDITSTATE            VARCHAR(1),
   REMARK               VARCHAR(2000),
   OWNERID              VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME ,
   PRIMARY KEY (KPIRECORDSUMMARYID)
);

ALTER TABLE T_HR_KPIRECORDPERSONSUMMARY COMMENT '绩效考核个人汇总记录';

/*==============================================================*/
/* Table: T_HR_KPITYPE                                          */
/*==============================================================*/
CREATE TABLE T_HR_KPITYPE
(
   KPITYPEID            VARCHAR(50) NOT NULL COMMENT 'KPI类别ID',
   SCORETYPEID          VARCHAR(50) COMMENT '评分方式ID',
   KPITYPENAME          VARCHAR(50) COMMENT 'KPI类别名称',
   KPITYPEREMARK        VARCHAR(2000) COMMENT 'KPI类别说明',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (KPITYPEID)
);

ALTER TABLE T_HR_KPITYPE COMMENT 'KPI类别';

/*==============================================================*/
/* Table: T_HR_LEAVEREFEROT                                     */
/*==============================================================*/
CREATE TABLE T_HR_LEAVEREFEROT
(
   RECORDID             VARCHAR(50) NOT NULL,
   LEAVE_TYPE_SETID     VARCHAR(50) NOT NULL COMMENT '请假类型',
   LEAVE_RECORDID       VARCHAR(50) COMMENT '请假记录',
   OVERTIME_RECORDID    VARCHAR(50) COMMENT '加班记录',
   LEAVE_APPLY_DATE     DATETIME  COMMENT '请假申请时间',
   LEAVE_TOTAL_DAYS     NUMERIC(8,2) DEFAULT 0 COMMENT '请假合计天数',
   LEAVE_TOTAL_HOURS    NUMERIC(8,2) DEFAULT 0 COMMENT '请假合计时长',
   ACTION               NUMERIC(8,0) DEFAULT 1 COMMENT '操作动作:1 请假 2 销假',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EFFECTDATE           DATETIME  COMMENT '生效时间，和T_HR_EmployeeLevelDayCount表中的EFFicdate，TeminateDate同步',
   EXPIREDATE           DATETIME  COMMENT '过期时间，和T_HR_EmployeeLevelDayCount表中的EFFicdate，TeminateDate同步',
   STATUS               NUMERIC(8,0) DEFAULT 0 COMMENT '0 无效，1 有效',
   LEAVE_CANCEL_RECORDID VARCHAR(50) COMMENT '销假记录ID',
   LEAVESTARTDATE       DATETIME  COMMENT '请假开始时间',
   LEAVEENDDATE         DATETIME  COMMENT '请假结束时间',
   LEAVETYPEVALUE       NUMERIC(8,0) COMMENT '假期类型',
   PRIMARY KEY (RECORDID)
);

ALTER TABLE T_HR_LEAVEREFEROT COMMENT '加班请假关系记录表';

/*==============================================================*/
/* Table: T_HR_LEAVETYPESET                                     */
/*==============================================================*/
CREATE TABLE T_HR_LEAVETYPESET
(
   LEAVETYPESETID       VARCHAR(50) NOT NULL COMMENT '请假类型ID',
   LEAVETYPENAME        VARCHAR(50) COMMENT '假期名称',
   LEAVETYPEVALUE       VARCHAR(50) COMMENT '假期值',
   ISFREELEAVEDAY       VARCHAR(1) COMMENT '是否带薪假:0:否,1是,当是带薪假的时候应该有带薪假子表记录',
   MAXDAYS              NUMERIC(8,0) COMMENT '最多请假天数',
   FINETYPE             VARCHAR(1) COMMENT '1,不扣(带薪假) 2、扣款；3、调休+扣款；4、调休+带薪假抵扣+扣款；',
   SEXRESTRICT          VARCHAR(1) COMMENT '性别限制：0女，1男，2不限',
   POSTLEVELRESTRICT    VARCHAR(50) COMMENT '享受的岗位级别：什么岗位级别以上的享受',
   ENTRYRESTRICT        VARCHAR(50) COMMENT '是否转正后才能享受',
   OFFESTTYPE           VARCHAR(50) COMMENT '调休方式',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   REMARK               VARCHAR(2000) COMMENT '备注',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   PRIMARY KEY (LEAVETYPESETID)
);

ALTER TABLE T_HR_LEAVETYPESET COMMENT '生成员工的假期
包括年假，每月享受的病假天数等
';

/*==============================================================*/
/* Table: T_HR_LEFTOFFICE                                       */
/*==============================================================*/
CREATE TABLE T_HR_LEFTOFFICE
(
   DIMISSIONID          VARCHAR(50) NOT NULL COMMENT '离职记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   LEFTOFFICEDATE       DATETIME COMMENT '离职时间',
   STOPPAYMENTDATE      DATETIME COMMENT '停薪日期',
   LEFTOFFICECATEGORY   VARCHAR(50) COMMENT '离职类别',
   LEFTOFFICEREASON     VARCHAR(500) COMMENT '离职原因',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   APPLYDATE            DATETIME COMMENT '申请时间',
   CONFIRMDATE          DATETIME COMMENT '确认时间',
   REMARK               VARCHAR(2000) COMMENT '离职说明',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   EMPLOYEEPOSTID       VARCHAR(50) COMMENT '员工岗位',
   ISAGENCY             VARCHAR(1) COMMENT '是否代理:0非代理,1代理岗位',
   PRIMARY KEY (DIMISSIONID)
);

ALTER TABLE T_HR_LEFTOFFICE COMMENT '员工离职记录(未加权限)';

/*==============================================================*/
/* Table: T_HR_LEFTOFFICECONFIRM                                */
/*==============================================================*/
CREATE TABLE T_HR_LEFTOFFICECONFIRM
(
   CONFIRMID            VARCHAR(50) NOT NULL COMMENT '离职确认ID',
   DIMISSIONID          VARCHAR(50) COMMENT '离职记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   STOPPAYMENTDATE      DATETIME COMMENT '停薪日期',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   CONFIRMDATE          DATETIME COMMENT '离职确认时间',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   LEFTOFFICEDATE       DATETIME COMMENT '离职时间',
   LEFTOFFICECATEGORY   VARCHAR(50) COMMENT '离职类别',
   LEFTOFFICEREASON     VARCHAR(500) COMMENT '离职原因',
   REMARK               VARCHAR(2000) COMMENT '离职说明',
   EMPLOYEECNAME        VARCHAR(50) COMMENT '员工中文名',
   APPLYDATE            DATETIME COMMENT '申请时间',
   EMPLOYEEPOSTID       VARCHAR(50) COMMENT '员工岗位',
   PRIMARY KEY (CONFIRMID)
);

ALTER TABLE T_HR_LEFTOFFICECONFIRM COMMENT '员工离职确认(未加权限)';

/*==============================================================*/
/* Table: T_HR_NOATTENDCARDEMPLOYEES                            */
/*==============================================================*/
CREATE TABLE T_HR_NOATTENDCARDEMPLOYEES
(
   NOATTENDCARDEMPLOYEESID VARCHAR(50) NOT NULL COMMENT '员工考勤方案ID',
   EMPLOYEENAME         VARCHAR(2000) COMMENT '员工姓名',
   EMPLOYEEID           VARCHAR(2000) COMMENT '员工ID',
   ALWAYS               VARCHAR(50) COMMENT '0：否 需要填开始结束时间，1：是',
   STARTDATE            DATETIME COMMENT '有效期开始时间',
   ENDDATE              DATETIME COMMENT '有效期结束时间',
   CHECKSTATE           VARCHAR(50) COMMENT '审核状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   PRIMARY KEY (NOATTENDCARDEMPLOYEESID)
);

ALTER TABLE T_HR_NOATTENDCARDEMPLOYEES COMMENT '免打卡员工设置';

/*==============================================================*/
/* Table: T_HR_ORGCHECKSUMMARY                                  */
/*==============================================================*/
CREATE TABLE T_HR_ORGCHECKSUMMARY
(
   ORGCHECKSUMMARYID    VARCHAR(50) NOT NULL,
   FATHERLID            VARCHAR(50),
   ISTOP                VARCHAR(50),
   ORDERNUMBER          NUMERIC(8,0),
   CHECKOBJTYPE         VARCHAR(1),
   CHECKOBJID           VARCHAR(50),
   CHECKMODELNAME       VARCHAR(500),
   PERFORMANCEGOAL      VARCHAR(2000),
   POINT                NUMERIC(8,0) COMMENT '考核分值：顶级考核模块手动设置，其他模块根据权重自动算出来',
   PERFORMYEAR          VARCHAR(50),
   PERFORMQUARTER       VARCHAR(50),
   PERFORMMOTH          VARCHAR(50),
   AVERAGE              NUMERIC(8,0),
   EXAMINEDATE          VARCHAR(50),
   EXAMINEPOINT         NUMERIC(8,0),
   LEADCOMMENT          VARCHAR(2000),
   CHECKSTATE           VARCHAR(1),
   REMARK               VARCHAR(2000),
   EDITSTATE            VARCHAR(1),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME,
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (ORGCHECKSUMMARYID)
);

ALTER TABLE T_HR_ORGCHECKSUMMARY COMMENT '组织考核模块汇总记录';

/*==============================================================*/
/* Table: T_HR_ORGANIZATIONCHECKMODEL                           */
/*==============================================================*/
CREATE TABLE T_HR_ORGANIZATIONCHECKMODEL
(
   ORGCHECKMODELID      VARCHAR(50) NOT NULL,
   FATHERLID            VARCHAR(50),
   ISTOP                VARCHAR(50),
   ORDERNUMBER          NUMERIC(8,0),
   CHECKOBJTYPE         VARCHAR(1),
   CHECKOBJID           VARCHAR(50),
   CHECKMODELNAME       VARCHAR(500),
   PERFORMANCEGOAL      VARCHAR(2000),
   WEIGHING             NUMERIC(8,0),
   POINT                NUMERIC(8,0) COMMENT '考核分值：顶级考核模块手动设置，其他模块根据权重自动算出来',
   REMARK               VARCHAR(2000),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME,
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (ORGCHECKMODELID)
);

ALTER TABLE T_HR_ORGANIZATIONCHECKMODEL COMMENT '组织考核模块设置';

/*==============================================================*/
/* Table: T_HR_OUTAPPLYCONFIRM                                  */
/*==============================================================*/
CREATE TABLE T_HR_OUTAPPLYCONFIRM
(
   OUTAPPLYCONFIRMID    VARCHAR(50) NOT NULL COMMENT '外出确认ID',
   OUTAPPLYID           VARCHAR(50) COMMENT '外出申请记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   STARTDATE            DATETIME COMMENT '外出确认开始日期',
   ENDDATE              DATETIME COMMENT '外出确认结束日期',
   OUTAPLLYTIMES        VARCHAR(50) COMMENT '外出时长',
   CHECKSTATE           VARCHAR(50) COMMENT '审核状态',
   OUTREPORT            VARCHAR(2000) COMMENT '外出报告',
   ISCANCELED           VARCHAR(50) COMMENT '0 否，1 是',
   CANCELREASON         VARCHAR(2000) COMMENT '取消原因',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (OUTAPPLYCONFIRMID)
);

ALTER TABLE T_HR_OUTAPPLYCONFIRM COMMENT '员工外出申请确认';

/*==============================================================*/
/* Table: T_HR_OUTAPPLYRECORD                                   */
/*==============================================================*/
CREATE TABLE T_HR_OUTAPPLYRECORD
(
   OUTAPPLYID           VARCHAR(50) NOT NULL COMMENT '外出申请记录ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   OVERTIMECATE         VARCHAR(50) COMMENT '加班生效方式
            0 审核通过的加班申请；1 超过工作时间外自动累计；2 仅节假日累计；',
   STARTDATE            DATETIME COMMENT '外出开始日期',
   ISSAMEDAYRETURN      VARCHAR(50) COMMENT '0 否，1 是',
   ENDDATE              DATETIME COMMENT '外出结束日期',
   OUTAPLLYTIMES        VARCHAR(2000) COMMENT '外出时长',
   ISCONFIRMED          VARCHAR(50) COMMENT '0：否，1 是',
   CHECKSTATE           VARCHAR(50) COMMENT '审核状态',
   REASON               VARCHAR(2000) COMMENT '外出事由',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (OUTAPPLYID)
);

ALTER TABLE T_HR_OUTAPPLYRECORD COMMENT '员工外出申请记录';

/*==============================================================*/
/* Table: T_HR_OUTPLANDAYS                                      */
/*==============================================================*/
CREATE TABLE T_HR_OUTPLANDAYS
(
   OUTPLANDAYID         VARCHAR(50) NOT NULL COMMENT '列外日期ID',
   VACATIONID           VARCHAR(50) COMMENT '工作日历ID',
   OUTPLANNAME          VARCHAR(50) COMMENT '列外日期名称',
   STARTDATE            DATETIME COMMENT '开始日期',
   ENDDATE              DATETIME COMMENT '结束日期',
   DAYS                 NUMERIC(8,0) COMMENT '工作日历天数',
   DAYTYPE              VARCHAR(1) COMMENT '工作日历类别,1 假期；2 工作日',
   ISADJUSTLEAVE        VARCHAR(1) COMMENT '是否调休',
   ISHALFDAY            VARCHAR(50) COMMENT '是否设置半天 0  否,1 是',
   PEROID               VARCHAR(50) COMMENT '上午下午：0 上午,1 下午',
   PRIMARY KEY (OUTPLANDAYID)
);

ALTER TABLE T_HR_OUTPLANDAYS COMMENT '列外日期';

/*==============================================================*/
/* Table: T_HR_OVERTIMEREWARD                                   */
/*==============================================================*/
CREATE TABLE T_HR_OVERTIMEREWARD
(
   OVERTIMEREWARDID     VARCHAR(50) NOT NULL COMMENT '加班报酬ID',
   OVERTIMEREWARDNAME   VARCHAR(50) COMMENT '加班报酬名',
   USUALOVERTIMEPAYRATE NUMERIC(8,0) COMMENT '平常加班报酬倍数',
   WEEKENDPAYRATE       NUMERIC(8,0) COMMENT '周日周末加班报酬倍数',
   VACATIONPAYRATE      NUMERIC(8,0) COMMENT '节假日加班报酬倍数',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (OVERTIMEREWARDID)
);

ALTER TABLE T_HR_OVERTIMEREWARD COMMENT '加班报酬表#';

/*==============================================================*/
/* Table: T_HR_PERFORMANCEDETAIL                                */
/*==============================================================*/
CREATE TABLE T_HR_PERFORMANCEDETAIL
(
   PERFORMANCEDETAILID  VARCHAR(50) NOT NULL COMMENT '关联ID',
   KPIRCORDID           VARCHAR(50) COMMENT 'KPI记录ID',
   PERFORMANCEID        VARCHAR(50) COMMENT '考核记录ID',
   PRIMARY KEY (PERFORMANCEDETAILID)
);

ALTER TABLE T_HR_PERFORMANCEDETAIL COMMENT '绩效和KPI记录关联表';

/*==============================================================*/
/* Table: T_HR_PERFORMANCERECORD                                */
/*==============================================================*/
CREATE TABLE T_HR_PERFORMANCERECORD
(
   PERFORMANCEID        VARCHAR(50) NOT NULL COMMENT '个人考核记录ID',
   SUMID                VARCHAR(50) COMMENT '绩效考核记录ID',
   PERFORMANCESCORE     NUMERIC(8,0) COMMENT '考核得分',
   PERFORMANCEREMARK    VARCHAR(2000) COMMENT '考核评语',
   APPRAISEEID          VARCHAR(50) COMMENT '被考核人ID',
   PRIMARY KEY (PERFORMANCEID)
);

ALTER TABLE T_HR_PERFORMANCERECORD COMMENT '绩效考核记录';

/*==============================================================*/
/* Table: T_HR_PERSONASSIGNSET                                  */
/*==============================================================*/
CREATE TABLE T_HR_PERSONASSIGNSET
(
   PERSONASSIGNSETID    VARCHAR(50) NOT NULL COMMENT '活动经费下拨设置ID',
   ASSIGNCOMPANYID      VARCHAR(50) COMMENT '活动经费下拨公司ID',
   ASSIGNCOMPANYNAME    VARCHAR(200) COMMENT '活动经费下拨公司名称',
   ASSIGNERID           VARCHAR(50) COMMENT '活动经费下拨人员工ID',
   ASSIGNERNAME         VARCHAR(200) COMMENT '活动经费下拨人姓名',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   ISEXPIRED            VARCHAR(1) COMMENT '0:不过期；1：过期；',
   PERSONASSIGNSETNAME  VARCHAR(50) COMMENT '经费下拨设置名称',
   PRIMARY KEY (PERSONASSIGNSETID)
);

ALTER TABLE T_HR_PERSONASSIGNSET COMMENT '活动经费下拨设置';

/*==============================================================*/
/* Table: T_HR_PENSIONALARMSET                                  */
/*==============================================================*/
CREATE TABLE T_HR_PENSIONALARMSET
(
   PENSIONSETID         VARCHAR(50) NOT NULL COMMENT '社保项目ID',
   COMPANYID            VARCHAR(50) COMMENT '社保公司',
   ALARMPAY             NUMERIC(38,0) COMMENT '每月几号提醒缴纳',
   ALARMDOWN            NUMERIC(38,0) COMMENT '每月几号提醒下载对账单',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (PENSIONSETID)
);

ALTER TABLE T_HR_PENSIONALARMSET COMMENT '社保提醒设置(未加权限)';

/*==============================================================*/
/* Table: T_HR_PENSIONDETAIL                                    */
/*==============================================================*/
CREATE TABLE T_HR_PENSIONDETAIL
(
   PENSIONDETAILID      VARCHAR(50) NOT NULL COMMENT '社保记录ID',
   PENSIONMASTERID      VARCHAR(50) COMMENT '员工社保档案ID',
   CARDID               VARCHAR(50) COMMENT '社保卡号',
   PENSIONYEAR          NUMERIC(8,0) COMMENT '社保年份',
   PENSIONMOTH          NUMERIC(8,0) COMMENT '社保月份',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   COMPUTERNO           VARCHAR(50) COMMENT '社保电脑号',
   PAYBASE              NUMERIC(8,0) COMMENT '缴交基数',
   TOTALCOST            NUMERIC(8,0) COMMENT '交缴总合计',
   TOTALPERSONCOST      NUMERIC(8,0) COMMENT '个人缴合计',
   TOTALCOMPANYCOST     NUMERIC(8,0) COMMENT '单位缴合计',
   PENSIONPERSONCOST    NUMERIC(8,0) COMMENT '养老保险个人缴',
   PENSIONCOMPANYCOST   NUMERIC(8,0) COMMENT '养老保险单位缴',
   HOUSINGFUNDCOMPANYCOST NUMERIC(8,0) COMMENT '住房公积金单位缴',
   MEDICAREPERSONCOST   NUMERIC(8,0) COMMENT '医疗保险个人缴',
   MEDICARECOMPANYCOST  NUMERIC(8,0) COMMENT '医疗保险单位缴',
   INJURYINSURANCECOMPANYCOST NUMERIC(8,0) COMMENT '工伤保险单位缴',
   UNEMPLOYEDINSURANCECOMPANYCOST NUMERIC(8,0) COMMENT '失业保险单位缴',
   BIRTHINSURANCECOMPANYCOST NUMERIC(8,0) COMMENT '生育保险单位缴',
   PAYDATE              DATETIME COMMENT '缴纳时间',
   COMPANYID            VARCHAR(500) COMMENT '缴纳公司',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   IDNUMBER             VARCHAR(50) COMMENT '身份证',
   PENSIONPERSONRATIO   NUMERIC(8,0) COMMENT '养老保险个人缴比例',
   PENSIONCOMPANYRATIO  NUMERIC(8,0) COMMENT '养老保险单位缴比例',
   HOUSINGFUNDCOMPANYRATIO NUMERIC(8,0) COMMENT '住房公积金单位缴比例',
   HOUSINGFUNDPERSONCOST NUMERIC(8,0) COMMENT '住房公积金个人缴',
   HOUSINGFUNDPERSONRATIO NUMERIC(8,0) COMMENT '住房公积金个人缴比例',
   MEDICAREPERSONRATIO  NUMERIC(8,0) COMMENT '医疗保险个人缴比例',
   MEDICARECOMPANYRATIO NUMERIC(8,0) COMMENT '医疗保险单位缴比例',
   INJURYINSURANCECOMPANYRATIO NUMERIC(8,0) COMMENT '工伤保险单位缴比例',
   INJURYINSURANCEPERSONCOST NUMERIC(8,0) COMMENT '工伤保险个人缴',
   INJURYINSURANCEPERSONRATIO NUMERIC(8,0) COMMENT '工伤保险个人缴比例',
   UNEMPLOYEDCOMPANYRATIO NUMERIC(8,0) COMMENT '失业保险单位缴比例',
   UNEMPLOYEDPERSON     NUMERIC(8,0) COMMENT '失业保险个人缴',
   UNEMPLOYEDPERSONRATIO NUMERIC(8,0) COMMENT '失业保险个人缴比例',
   BIRTHCOMPANYRATIO    NUMERIC(8,0) COMMENT '生育保险单位缴比例',
   BIRTHPERSONCOST      NUMERIC(8,0) COMMENT '生育保险个人缴',
   BIRTHPERSONRATIO     NUMERIC(8,0) COMMENT '生育保险个人缴比例',
   PENSIONBASE          NUMERIC(8,0) COMMENT '养老缴交基数',
   INJURYBASE           NUMERIC(8,0) COMMENT '工伤缴交基数',
   MEDICAREBASE         NUMERIC(8,0) COMMENT '医疗缴交基数',
   BIRTHBASE            NUMERIC(8,0) COMMENT '生育缴交基数',
   UNEMPLOYEDBASE       NUMERIC(8,0) COMMENT '失业缴交基数',
   HOUSINGFUNDBASE      NUMERIC(8,0) COMMENT '住房公积金缴交基数',
   CADRECOMPANYCOST     NUMERIC(8,0) COMMENT '老干部单位交',
   CADRECOMPANYRATIO    NUMERIC(8,0) COMMENT '老干部单位比例',
   SUBSIDYCOMPANYCOST   NUMERIC(8,0) COMMENT '大额补助单位交',
   SUBSIDYCOMPANYRATIO  NUMERIC(8,0) COMMENT '大额补助单位交比例',
   SUBSIDYPERSONCOST    NUMERIC(8,0) COMMENT '大额补助个人交',
   SUBSIDYPERSONRATIO   NUMERIC(8,0) COMMENT '大额补助个人交比例',
   ISLOCAL              VARCHAR(10) COMMENT '是否本地户口',
   PRIMARY KEY (PENSIONDETAILID)
);

ALTER TABLE T_HR_PENSIONDETAIL COMMENT '员工社保记录#';

/*==============================================================*/
/* Table: T_HR_PENSIONMASTER                                    */
/*==============================================================*/
CREATE TABLE T_HR_PENSIONMASTER
(
   PENSIONMASTERID      VARCHAR(50) NOT NULL COMMENT '员工社保档案ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   CARDID               VARCHAR(50) COMMENT '社保卡号',
   COMPUTERNO           VARCHAR(50) COMMENT '社保电脑号',
   PENSIONCITY          VARCHAR(100) COMMENT '社保所在地城市',
   STARTDATE            DATETIME COMMENT '开始交缴时间',
   LASTDATE             DATETIME COMMENT '最后一次缴纳时间',
   ISVALID              VARCHAR(50) COMMENT '0 无效
            1 有效',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   SOCIALSERVICEYEAR    VARCHAR(50) COMMENT '社会司龄年',
   SOCIALSERVICEMONTH   VARCHAR(50) COMMENT '社会司龄月',
   SOCIALSERVICE        DATETIME COMMENT '社保购买起始时间',
   PRIMARY KEY (PENSIONMASTERID)
);

ALTER TABLE T_HR_PENSIONMASTER COMMENT 'EmploymentSocialInsurance';

/*==============================================================*/
/* Table: T_HR_PERFORMANCEREWARDRECORD                          */
/*==============================================================*/
CREATE TABLE T_HR_PERFORMANCEREWARDRECORD
(
   PERFORMANCEREWARDRECORDID VARCHAR(50) NOT NULL COMMENT '绩效奖金记录ID',
   PERFORMANCESUM       NUMERIC(8,0) COMMENT '绩效金额',
   EMPLOYEEID           VARCHAR(50) NOT NULL COMMENT '员工ID',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   SALARYMONTH          VARCHAR(50) COMMENT '所属月份',
   SALARYYEAR           VARCHAR(50) COMMENT '所属年份',
   GENERATETYPE         VARCHAR(50) COMMENT '0自动生成的，1手动输入的',
   CHECKSTATE           VARCHAR(50) COMMENT '审核状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PERFORMANCESCORE     NUMERIC(8,0) COMMENT '绩效分数',
   STARTDATE            DATETIME COMMENT '考核开始日期',
   ENDDATE              DATETIME COMMENT '考核结束日期',
   PRIMARY KEY (PERFORMANCEREWARDRECORDID)
);

ALTER TABLE T_HR_PERFORMANCEREWARDRECORD COMMENT '绩效奖金记录';

/*==============================================================*/
/* Table: T_HR_PERFORMANCEREWARDSET                             */
/*==============================================================*/
CREATE TABLE T_HR_PERFORMANCEREWARDSET
(
   PERFORMANCEREWARDSETID VARCHAR(50) NOT NULL COMMENT '绩效奖金ID',
   PERFORMANCECATEGORY  VARCHAR(50) COMMENT '绩效类型',
   PERFORMANCECAPITAL   NUMERIC(8,0) COMMENT '绩效系数',
   CALCULATETYPE        VARCHAR(50) COMMENT '0固定值，1计算值',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   PRIMARY KEY (PERFORMANCEREWARDSETID)
);

ALTER TABLE T_HR_PERFORMANCEREWARDSET COMMENT '绩效奖金设置';

/*==============================================================*/
/* Table: T_HR_POST                                             */
/*==============================================================*/
CREATE TABLE T_HR_POST
(
   POSTID               VARCHAR(50) NOT NULL,
   POSTDICTIONARYID     VARCHAR(50),
   COMPANYID            VARCHAR(50),
   DEPARTMENTID         VARCHAR(50),
   DEPARTMENTNAME       VARCHAR(100),
   POSTFUNCTION         VARCHAR(2000),
   POSTNUMBER           INT,
   POSTLEVEL            NUMERIC(8,0),
   POSTCOEFFICIENT      VARCHAR(50),
   SALARYLEVEL          NUMERIC(8,0),
   POSTGOAL             VARCHAR(500),
   FATHERPOSTID         VARCHAR(50),
   UNDERNUMBER          INT,
   PROMOTEDIRECTION     VARCHAR(50),
   CHANGEPOST           VARCHAR(50),
   CHECKSTATE           VARCHAR(1),
   EDITSTATE            VARCHAR(1),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME,
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME,
   CREATEPOSTID         VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   PRIMARY KEY (POSTID)
);

ALTER TABLE T_HR_POST COMMENT '岗位#';

/*==============================================================*/
/* Table: T_HR_POSTDICTIONARY                                   */
/*==============================================================*/
CREATE TABLE T_HR_POSTDICTIONARY
(
   POSTDICTIONARYID     VARCHAR(50) NOT NULL COMMENT '岗位字典ID',
   DEPARTMENTDICTIONARYID VARCHAR(50) COMMENT '部门字典ID',
   POSTCODE             VARCHAR(50) COMMENT '岗位编号',
   POSTNAME             VARCHAR(50) COMMENT '岗位名称',
   POSTFUNCTION         VARCHAR(2000) COMMENT '岗位职责',
   POSTLEVEL            NUMERIC(8,0) COMMENT '岗位级别',
   SALARYLEVEL          VARCHAR(50) COMMENT '薪资等级',
   PROMOTEDIRECTION     VARCHAR(50) COMMENT '晋升方向',
   CHANGEPOST           VARCHAR(50) COMMENT '轮换岗位',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   CHECKSTATE           VARCHAR(50) COMMENT '审核状态',
   EDITSTATE            VARCHAR(50) COMMENT '编辑状态',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   PRIMARY KEY (POSTDICTIONARYID)
);

ALTER TABLE T_HR_POSTDICTIONARY COMMENT '岗位字典';

/*==============================================================*/
/* Table: T_HR_POSTHISTORY                                      */
/*==============================================================*/
CREATE TABLE T_HR_POSTHISTORY
(
   RECORDSID            VARCHAR(50) NOT NULL COMMENT '记录ID',
   POSTDICTIONARYID     VARCHAR(50) COMMENT '岗位字典ID',
   POSTID               VARCHAR(50) COMMENT '岗位ID',
   DEPARTMENTID         VARCHAR(50) COMMENT '部门ID',
   DEPARTMENTNAME       VARCHAR(100) COMMENT '部门名称',
   COMPANYID            VARCHAR(100) COMMENT '公司名称',
   POSTFUNCTION         VARCHAR(2000) COMMENT '岗位职责',
   POSTNUMBER           NUMERIC(38,0) COMMENT '人员编制',
   POSTGOAL             VARCHAR(100) COMMENT '岗位目标',
   CHECKUSER            VARCHAR(50) COMMENT '审核人',
   FATHERPOSTID         VARCHAR(50) COMMENT '直接上级',
   UNDERNUMBER          NUMERIC(38,0) COMMENT '下属人数',
   PROMOTEDIRECTION     VARCHAR(100) COMMENT '晋升方向',
   CHANGEPOST           VARCHAR(50) COMMENT '轮换岗位',
   EDITSTATE            VARCHAR(50) COMMENT '编辑状态',
   REUSEDATE            DATETIME COMMENT '生效时间',
   CANCELDATE           DATETIME COMMENT '撤销时间',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   POSTLEVEL            NUMERIC(8,0) COMMENT '岗位级别',
   CHECKSTATE           VARCHAR(50) COMMENT '审核状态',
   POSTCOEFFICIENT      VARCHAR(50) COMMENT '岗位系数',
   PRIMARY KEY (RECORDSID)
);

ALTER TABLE T_HR_POSTHISTORY COMMENT '岗位历史记录#';

/*==============================================================*/
/* Table: T_HR_POSTLEVELDISTINCTION                             */
/*==============================================================*/
CREATE TABLE T_HR_POSTLEVELDISTINCTION
(
   POSTLEVELID          VARCHAR(50) NOT NULL COMMENT '岗位职级ID',
   SALARYSYSTEMID       VARCHAR(50) COMMENT '薪酬体系表ID',
   BASICSALARY          NUMERIC(8,0) COMMENT '初级薪资',
   LEVELBALANCE         NUMERIC(8,0) COMMENT '级差额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   POSTLEVEL            NUMERIC(8,0) COMMENT '岗位等级',
   PRIMARY KEY (POSTLEVELID)
);

ALTER TABLE T_HR_POSTLEVELDISTINCTION COMMENT '岗位职级表';

/*==============================================================*/
/* Table: T_HR_RAMDONGROUPPERSON                                */
/*==============================================================*/
CREATE TABLE T_HR_RAMDONGROUPPERSON
(
   GROUPPERSONID        VARCHAR(50) NOT NULL COMMENT '关联ID',
   RANDOMGROUPID        VARCHAR(50) COMMENT '抽查组ID',
   PERSONID             VARCHAR(50) COMMENT '人员ID',
   PRIMARY KEY (GROUPPERSONID)
);

ALTER TABLE T_HR_RAMDONGROUPPERSON COMMENT '抽查组人员';

/*==============================================================*/
/* Table: T_HR_RANDOMGROUP                                      */
/*==============================================================*/
CREATE TABLE T_HR_RANDOMGROUP
(
   RANDOMGROUPID        VARCHAR(50) NOT NULL COMMENT '抽查组ID',
   RANDOMGROUPNAME      VARCHAR(50) COMMENT '抽查组名称',
   GROUPCOUNT           NUMERIC(8,0) COMMENT '抽查组人数',
   GROUPREMARK          VARCHAR(2000) COMMENT '抽查组说明',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (RANDOMGROUPID)
);

ALTER TABLE T_HR_RANDOMGROUP COMMENT '抽查组';

/*==============================================================*/
/* Table: T_HR_RELATIONPOST                                     */
/*==============================================================*/
CREATE TABLE T_HR_RELATIONPOST
(
   RECORDID             VARCHAR(50) NOT NULL,
   RELATIONPOSTID       VARCHAR(50) NOT NULL COMMENT '关联表ID',
   POSTID               VARCHAR(50) COMMENT '岗位ID',
   RELATEPOSTID         VARCHAR(50) COMMENT '关联岗位ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (RELATIONPOSTID, RECORDID)
);

ALTER TABLE T_HR_RELATIONPOST COMMENT '关联岗位表';

/*==============================================================*/
/* Table: T_HR_RESUME                                           */
/*==============================================================*/
CREATE TABLE T_HR_RESUME
(
   RESUMEID             VARCHAR(50) NOT NULL COMMENT '简历id',
   WISHCOMPANY          VARCHAR(300) COMMENT '意向公司',
   WISHPOST             VARCHAR(100) COMMENT '意向岗位',
   WISHAREA             VARCHAR(100) COMMENT '意向地区',
   NAME                 VARCHAR(50) COMMENT '姓名',
   SEX                  VARCHAR(10) COMMENT '性别',
   NATION               VARCHAR(50) COMMENT '民族',
   PROVINCE             VARCHAR(100) COMMENT '籍贯',
   HEIGHT               VARCHAR(50) COMMENT '身高',
   MARRIAGE             VARCHAR(1) COMMENT '婚姻状况',
   IDCARDNUMBER         VARCHAR(50) COMMENT '身份证号码',
   POLITICS             VARCHAR(100) COMMENT '政治面貌',
   REGRESIDENCE         VARCHAR(200) COMMENT '户口所在地',
   EMAIL                VARCHAR(50) COMMENT '电子邮件',
   MOBILE               VARCHAR(50) COMMENT '手机号码',
   CURRENTADRESS        VARCHAR(100) COMMENT '目前居住地',
   URGENCYPERSON        VARCHAR(50) COMMENT '紧急联系人',
   URGENCYCONTACT       VARCHAR(100) COMMENT '紧急联系方式',
   FAMILYADDRESS        VARCHAR(100) COMMENT '家庭详细地址',
   FAMILYZIPCODE        VARCHAR(100) COMMENT '家庭邮政编码',
   PHOTO                LONGBLOB COMMENT '照片',
   TIPTOPEDUCATION      VARCHAR(100) COMMENT '最高学历',
   EXPECTATIONSALARY    VARCHAR(50) COMMENT '期望薪资',
   HAVECOMRELATION      VARCHAR(10) COMMENT '是否有亲属',
   COMRELATIONINFO      VARCHAR(100) COMMENT '亲属公司信息',
   SELFAPPRAISE         VARCHAR(300) COMMENT '自我评价',
   SCHOOLENCOURAGE      VARCHAR(500) COMMENT '校内奖励',
   OUTENCOURAGE         VARCHAR(500) COMMENT '校外奖励',
   GRADUATESCHOOL       VARCHAR(100) COMMENT '毕业学校',
   SPECIALTY            VARCHAR(300) COMMENT '所学专业',
   ENGLISHLEVEL         VARCHAR(20) COMMENT '英语水平',
   MAJORS               VARCHAR(500) COMMENT '主修课程',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   GRADUATIONDATE       DATETIME COMMENT '毕业时间',
   PRIMARY KEY (RESUMEID)
);

ALTER TABLE T_HR_RESUME COMMENT '简历库';

/*==============================================================*/
/* Table: T_HR_SALARYANDREWARDSUMMARYRD                         */
/*==============================================================*/
CREATE TABLE T_HR_SALARYANDREWARDSUMMARYRD
(
   RECORDID             VARCHAR(50) NOT NULL COMMENT '统计记录ID',
   COMPANYID            VARCHAR(50) NOT NULL COMMENT '公司ID',
   COMPANYNAME          VARCHAR(50) COMMENT '公司名称',
   BALANCEYEAR          VARCHAR(50) COMMENT '统计年份',
   BALANCEMONTH         VARCHAR(50) COMMENT '统计月份',
   TOTALSUM             NUMERIC(8,0) COMMENT '统计金额',
   GENERATETYPE         VARCHAR(50) COMMENT '0自动生成的，1手动输入的',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (RECORDID)
);

ALTER TABLE T_HR_SALARYANDREWARDSUMMARYRD COMMENT '薪资及福利成本(社保)统计记录';

/*==============================================================*/
/* Table: T_HR_SALARYSOLUTIONSTANDARD                           */
/*==============================================================*/
CREATE TABLE T_HR_SALARYSOLUTIONSTANDARD
(
   SOLUTIONSTANDARDID   VARCHAR(50) NOT NULL COMMENT '薪资方案标准ID',
   SALARYSTANDARDID     VARCHAR(50) COMMENT '薪资标准ID',
   SALARYSOLUTIONID     VARCHAR(50) COMMENT '薪资方案ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (SOLUTIONSTANDARDID)
);

ALTER TABLE T_HR_SALARYSOLUTIONSTANDARD COMMENT '薪资方案标准（删除）';

/*==============================================================*/
/* Table: T_HR_SCORETYPE                                        */
/*==============================================================*/
CREATE TABLE T_HR_SCORETYPE
(
   SCORETYPEID          VARCHAR(50) NOT NULL COMMENT '评分方式ID',
   RANDOMGROUPID        VARCHAR(50) COMMENT '抽查组ID',
   ISSYSTEMSCORE        VARCHAR(1) COMMENT '是否系统评分',
   SYSTEMWEIGHT         NUMERIC(8,0) COMMENT '系统评分权重',
   ISMANUALSCORE        VARCHAR(1) COMMENT '是否手动评分',
   MANUALWEIGHT         NUMERIC(8,0) COMMENT '手动评分权重',
   ISRANDOMSCORE        VARCHAR(1) COMMENT '是否抽查打分',
   RANDOMWEIGHT         NUMERIC(8,0) COMMENT '抽查权重',
   COUNTUNIT            NUMERIC(8,0) COMMENT '计算的单位天数',
   ADDSCORE             NUMERIC(8,0) COMMENT '提前单位天数加分',
   REDUCESCORE          NUMERIC(8,0) COMMENT '延后单位天数减分',
   MAXSCORE             NUMERIC(8,0) COMMENT '分数上限',
   MINSCORE             NUMERIC(8,0) COMMENT '分数下限',
   INITIALPOINT         NUMERIC(8,0) COMMENT '计分点天数',
   INITIALSCORE         NUMERIC(8,0) COMMENT '原始计分:内容为天数。以考核点激活后的n天为计分原点，在此基础上计算机打时是提前了多少天或延后了多少天。',
   INITAILSCORE         NUMERIC(8,0) COMMENT '内容为天数。以考核点激活后的n天为计分原点，在此基础上计算机打时是提前了多少天或延后了多少天。',
   LATERUNIT            NUMERIC(8,0) COMMENT '延迟小时数',
   PRIMARY KEY (SCORETYPEID)
);

ALTER TABLE T_HR_SCORETYPE COMMENT '评分方式';

/*==============================================================*/
/* Table: T_HR_SUMPERFORMANCERECORD                             */
/*==============================================================*/
CREATE TABLE T_HR_SUMPERFORMANCERECORD
(
   SUMID                VARCHAR(50) NOT NULL COMMENT '绩效考核记录ID',
   SUMPERSONID          VARCHAR(50) COMMENT '汇总人员ID',
   REVIEWERID           VARCHAR(50) COMMENT '审核人员ID',
   SUMNAME              VARCHAR(200) COMMENT '绩效考核名称',
   SUMSTART             DATETIME COMMENT '考核开始时间',
   SUMEND               DATETIME COMMENT '考核结束时间',
   SUMCOUNT             NUMERIC(8,0) COMMENT '考核人数',
   SUMSCORE             NUMERIC(8,0) COMMENT '考核得分',
   SUMREMARK            VARCHAR(2000) COMMENT '考核说明',
   SUMTYPE              VARCHAR(1) COMMENT '1.流程；2.任务。',
   SUMDATE              DATETIME COMMENT '汇总时间',
   BASEMONEY            NUMERIC(8,0) COMMENT '奖金基数',
   CHECKSTATE           VARCHAR(1) COMMENT '0：没有审核；1：正在审核；2：审核通过；3：审核不通过。',
   REVIEWREMARK         VARCHAR(2000) COMMENT '审核评语',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   SUMDEPTID            VARCHAR(50) COMMENT '汇总部门ID',
   AWARDTYPE            VARCHAR(1) COMMENT '奖金类型',
   PRIMARY KEY (SUMID)
);

ALTER TABLE T_HR_SUMPERFORMANCERECORD COMMENT '绩效考核汇总记录';

/*==============================================================*/
/* Table: T_HR_SALARYARCHIVE                                    */
/*==============================================================*/
CREATE TABLE T_HR_SALARYARCHIVE
(
   SALARYARCHIVEID      VARCHAR(50) NOT NULL COMMENT '薪资档案ID',
   SALARYSOLUTIONID     VARCHAR(50) COMMENT '薪资方案ID',
   SALARYSTANDARDID     VARCHAR(50) COMMENT '薪资标准ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   BASESALARY           NUMERIC(8,0) COMMENT '基本工资',
   POSTSALARY           NUMERIC(8,0) COMMENT '岗位工资',
   SECURITYALLOWANCE    NUMERIC(8,0) COMMENT '保密津贴',
   HOUSINGALLOWANCE     NUMERIC(8,0) COMMENT '住房津贴',
   AREADIFALLOWANCE     NUMERIC(8,0) COMMENT '地区差异补贴',
   FOODALLOWANCE        NUMERIC(8,0) COMMENT '餐费补贴',
   OTHERADDDEDUCT       NUMERIC(8,0) COMMENT '其他加扣款',
   OTHERADDDEDUCTDESC   VARCHAR(2000) COMMENT '加扣款说明',
   HOUSINGALLOWANCEDEDUCT NUMERIC(8,0) COMMENT '住房津贴扣款',
   PERSONALSIRATIO      NUMERIC(8,0) COMMENT '个人社保比',
   PERSONALINCOMERATIO  NUMERIC(8,0) COMMENT '个人所得税比',
   OTHERSUBJOIN         NUMERIC(8,0) COMMENT '其它代扣款',
   OTHERSUBJOINDESC     VARCHAR(50) COMMENT '代扣说明',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   CHECKSTATE           VARCHAR(50) COMMENT '审核状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属者',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   SALARYSOLUTIONNAME   VARCHAR(500) COMMENT '薪资方案名称',
   OVERYEAR             NUMERIC(8,0) COMMENT '终止年份',
   OVERMONTH            NUMERIC(8,0) COMMENT '终止月份',
   POSTLEVEL            NUMERIC(8,0) COMMENT '岗位级别',
   SALARYLEVEL          NUMERIC(8,0) COMMENT '薪资级别',
   BALANCE              NUMERIC(8,0) COMMENT '差额',
   OLDPOSTLEVEL         NUMERIC(8,0) COMMENT '异动前岗位级别',
   OLDSALARYLEVEL       NUMERIC(8,0) COMMENT '异动前薪资级别',
   PAYCOMPANY           VARCHAR(50) COMMENT '发薪公司',
   EMPLOYEEPOSTID       VARCHAR(50) COMMENT '岗位关联ID',
   ATTENDANCEORGID      VARCHAR(50) COMMENT '考勤机构ID',
   ATTENDANCEORGNAME    VARCHAR(100) COMMENT '考勤机构名称',
   BALANCEPOSTID        VARCHAR(50) COMMENT '结算岗位ID',
   BALANCEPOSTNAME      VARCHAR(300) COMMENT '结算岗位名称',
   SKILLPOSTLEVEL       VARCHAR(50) COMMENT '新技能岗位级别',
   SKILLSALARYLEVEL     VARCHAR(50) COMMENT '新技能薪资级别',
   PRIMARY KEY (SALARYARCHIVEID)
);

ALTER TABLE T_HR_SALARYARCHIVE COMMENT '薪资档案';

/*==============================================================*/
/* Table: T_HR_SALARYARCHIVEHIS                                 */
/*==============================================================*/
CREATE TABLE T_HR_SALARYARCHIVEHIS
(
   SALARYARCHIVEID      VARCHAR(50) NOT NULL COMMENT '薪资档案ID',
   SALARYSOLUTIONID     VARCHAR(50) NOT NULL COMMENT '薪资方案ID',
   SALARYSTANDARDID     VARCHAR(50) COMMENT '薪资标准ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   BASESALARY           NUMERIC(8,0) COMMENT '基本工资',
   POSTSALARY           NUMERIC(8,0) COMMENT '岗位工资',
   SECURITYALLOWANCE    NUMERIC(8,0) COMMENT '保密津贴',
   HOUSINGALLOWANCE     NUMERIC(8,0) COMMENT '住房津贴',
   AREADIFALLOWANCE     NUMERIC(8,0) COMMENT '地区差异补贴',
   FOODALLOWANCE        NUMERIC(8,0) COMMENT '餐费补贴',
   OTHERADDDEDUCT       NUMERIC(8,0) COMMENT '其他加扣款',
   OTHERADDDEDUCTDESC   VARCHAR(50) COMMENT '加扣款说明',
   HOUSINGALLOWANCEDEDUCT NUMERIC(8,0) COMMENT '住房津贴扣款',
   PERSONALSIRATIO      NUMERIC(8,0) COMMENT '个人社保比',
   PERSONALINCOMERATIO  NUMERIC(8,0) COMMENT '个人所得税比',
   OTHERSUBJOIN         NUMERIC(8,0) COMMENT '其它代扣款',
   OTHERSUBJOINDESC     VARCHAR(50) COMMENT '代扣说明',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   SALARYSOLUTIONNAME   VARCHAR(50) COMMENT '薪资方案名',
   POSTLEVEL            NUMERIC(8,0) COMMENT '岗位级别',
   SALARYLEVEL          NUMERIC(8,0) COMMENT '薪资级别',
   BALANCE              NUMERIC(8,0) COMMENT '差额',
   PRIMARY KEY (SALARYARCHIVEID)
);

ALTER TABLE T_HR_SALARYARCHIVEHIS COMMENT '薪资档案历史';

/*==============================================================*/
/* Table: T_HR_SALARYARCHIVEHISITEM                             */
/*==============================================================*/
CREATE TABLE T_HR_SALARYARCHIVEHISITEM
(
   SALARYARCHIVEITEMID  VARCHAR(50) NOT NULL COMMENT '薪资档案薪资项ID',
   SALARYARCHIVEID      VARCHAR(50) COMMENT '薪资档案ID',
   SALARYSTANDARDID     VARCHAR(50) COMMENT '薪资标准ID',
   SALARYITEMID         VARCHAR(50) COMMENT '薪资项ID',
   CALCULATEFORMULA     VARCHAR(2000) COMMENT '计算公式',
   CALCULATEFORMULACODE VARCHAR(2000) COMMENT '计算公式编码',
   SUM                  VARCHAR(2000) COMMENT '金额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   ORDERNUMBER          NUMERIC(8,0) COMMENT '排序号',
   PRIMARY KEY (SALARYARCHIVEITEMID)
);

ALTER TABLE T_HR_SALARYARCHIVEHISITEM COMMENT '薪资档案历史薪资项(#)';

/*==============================================================*/
/* Table: T_HR_SALARYARCHIVEITEM                                */
/*==============================================================*/
CREATE TABLE T_HR_SALARYARCHIVEITEM
(
   SALARYARCHIVEITEM    VARCHAR(50) NOT NULL COMMENT '薪资档案薪资项ID',
   SALARYARCHIVEITEMID  VARCHAR(50) NOT NULL,
   SALARYARCHIVEID      VARCHAR(50) COMMENT '薪资档案ID',
   SALARYSTANDARDID     VARCHAR(50) COMMENT '薪资标准ID',
   SALARYITEMID         VARCHAR(50) COMMENT '薪资项ID',
   CALCULATEFORMULA     VARCHAR(2000) COMMENT '计算公式',
   CALCULATEFORMULACODE VARCHAR(2000) COMMENT '计算公式编码',
   SUM                  VARCHAR(2000) COMMENT '金额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   ORDERNUMBER          NUMERIC(8,0) COMMENT '排序号',
   PRIMARY KEY (SALARYARCHIVEITEM, SALARYARCHIVEITEMID)
);

ALTER TABLE T_HR_SALARYARCHIVEITEM COMMENT '薪资档案薪资项(#)';

/*==============================================================*/
/* Table: T_HR_SALARYITEM                                       */
/*==============================================================*/
CREATE TABLE T_HR_SALARYITEM
(
   SALARYITEMID         VARCHAR(50) NOT NULL COMMENT '薪资项ID',
   SALARYITEMCODE       VARCHAR(50) COMMENT '薪资项Code',
   SALARYITEMNAME       VARCHAR(50) COMMENT '薪资项名称',
   SALARYITEMTYPE       VARCHAR(50) COMMENT '计算项类型:定薪类,考勤类,绩效类.',
   CALCULATORTYPE       VARCHAR(50) COMMENT '1、手工录入 ；
            2、薪资档案中输入；
            3、计算公式；',
   CALCULATEFORMULA     VARCHAR(2000) COMMENT '计算公式',
   CALCULATEFORMULACODE VARCHAR(2000) COMMENT '计算公式编码',
   GUERDONSUM           NUMERIC(8,0) COMMENT '金额',
   ENTITYNAME           VARCHAR(50) COMMENT '对应的实体名',
   ENTITYCODE           VARCHAR(50) COMMENT '对应的实体编码',
   ENTITYCOLUMNNAME     VARCHAR(100) COMMENT '对应的实体字段名',
   ENTITYCOLUMNCODE     VARCHAR(100) COMMENT '对应的实体字段编码',
   ISAUTOGENERATE       VARCHAR(1) COMMENT '薪资生成时计算值',
   MUSTSELECTED         VARCHAR(1) COMMENT '薪资标准必选项',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (SALARYITEMID)
);

ALTER TABLE T_HR_SALARYITEM COMMENT '薪资计算项设置(#)';

/*==============================================================*/
/* Table: T_HR_SALARYLEVEL                                      */
/*==============================================================*/
CREATE TABLE T_HR_SALARYLEVEL
(
   SALARYLEVELID        VARCHAR(50) NOT NULL COMMENT '薪资等级ID',
   POSTLEVELID          VARCHAR(50) COMMENT '岗位职级ID',
   SALARYLEVEL          VARCHAR(50) COMMENT '薪资等级',
   SALARYSUM            NUMERIC(8,0) COMMENT '薪资金额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (SALARYLEVELID)
);

ALTER TABLE T_HR_SALARYLEVEL COMMENT '薪资等级表';

/*==============================================================*/
/* Table: T_HR_SALARYRECORDBATCH                                */
/*==============================================================*/
CREATE TABLE T_HR_SALARYRECORDBATCH
(
   MONTHLYBATCHID       VARCHAR(50) NOT NULL COMMENT '月度批量结算ID',
   BALANCEYEAR          NUMERIC(8,0) COMMENT '结算年份',
   BALANCEMONTH         NUMERIC(8,0) COMMENT '结算月份',
   BALANCEDATE          DATETIME COMMENT '结算日期',
   BALANCEOBJECTTYPE    VARCHAR(1) COMMENT '结算对象类型',
   BALANCEOBJECTID      VARCHAR(50) COMMENT '结算对象Id',
   BALANCEOBJECTNAME    VARCHAR(500) COMMENT '结算对象名',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   REMARK               VARCHAR(2000) COMMENT '备注',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (MONTHLYBATCHID)
);

ALTER TABLE T_HR_SALARYRECORDBATCH COMMENT '薪资记录批量审核表';

/*==============================================================*/
/* Table: T_HR_SALARYSOLUTION                                   */
/*==============================================================*/
CREATE TABLE T_HR_SALARYSOLUTION
(
   SALARYSOLUTIONID     VARCHAR(50) NOT NULL COMMENT '薪资方案ID',
   AREADIFFERENCEID     VARCHAR(50) COMMENT '地区分类ID',
   SALARYSYSTEMID       VARCHAR(50) COMMENT '薪酬体系表ID',
   SALARYSOLUTIONNAME   VARCHAR(500) COMMENT '薪资方案名',
   SALARYCOUNTDAY       VARCHAR(50) COMMENT '薪资结算日',
   SALARYCOUNTALERTDAY  VARCHAR(50) COMMENT '结算提醒天数',
   PAYDAY               VARCHAR(50) COMMENT '薪资发放日',
   BANKNAME             VARCHAR(50) COMMENT '薪资银行',
   BANKACCOUNTNO        VARCHAR(50) COMMENT '薪资银行帐号',
   PAYTYPE              VARCHAR(50) COMMENT '0银行代发，1现金，这个是用来做默认设置的',
   PAYALERTDAY          VARCHAR(50) COMMENT '发放提醒天数',
   SALARYPRECISION      VARCHAR(50) COMMENT '薪资精度',
   TAXESBASIC           NUMERIC(8,0) COMMENT '纳税基数',
   TAXESCOSTRATE        NUMERIC(8,0) COMMENT '纳税系数',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   MINIMUMSALARY        VARCHAR(2000) COMMENT '最低工资',
   PRIMARY KEY (SALARYSOLUTIONID)
);

ALTER TABLE T_HR_SALARYSOLUTION COMMENT '薪资方案';

/*==============================================================*/
/* Table: T_HR_SALARYSOLUTIONASSIGN                             */
/*==============================================================*/
CREATE TABLE T_HR_SALARYSOLUTIONASSIGN
(
   SALARYSOLUTIONASSIGNID VARCHAR(50) NOT NULL COMMENT '薪资方案应用ID',
   ASSIGNEDOBJECTID     VARCHAR(50) COMMENT '分配对像ID',
   ASSIGNEDOBJECTTYPE   VARCHAR(50) COMMENT '分配对像类别',
   SALARYSOLUTIONID     VARCHAR(50) COMMENT '薪资方案ID',
   STARTDATE            VARCHAR(50) COMMENT '有效期开始时间',
   ENDDATE              VARCHAR(50) COMMENT '有效期结束时间',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (SALARYSOLUTIONASSIGNID)
);

ALTER TABLE T_HR_SALARYSOLUTIONASSIGN COMMENT '薪资方案应用';

/*==============================================================*/
/* Table: T_HR_SALARYSOLUTIONITEM                               */
/*==============================================================*/
CREATE TABLE T_HR_SALARYSOLUTIONITEM
(
   SOLUTIONITEMID       VARCHAR(50) NOT NULL COMMENT '薪资方案薪资项ID',
   SALARYITEMID         VARCHAR(50) COMMENT '薪资项ID',
   SALARYSOLUTIONID     VARCHAR(50) COMMENT '薪资方案ID',
   ORDERNUMBER          NUMERIC(8,0) COMMENT '排序号',
   PRIMARY KEY (SOLUTIONITEMID)
);

ALTER TABLE T_HR_SALARYSOLUTIONITEM COMMENT '薪资方案薪资项(#)';

/*==============================================================*/
/* Table: T_HR_SALARYSTANDARD                                   */
/*==============================================================*/
CREATE TABLE T_HR_SALARYSTANDARD
(
   SALARYSTANDARDID     VARCHAR(50) NOT NULL COMMENT '薪资标准ID',
   SALARYSOLUTIONID     VARCHAR(50) COMMENT '薪资方案ID',
   SALARYSTANDARDNAME   VARCHAR(200) COMMENT '薪资标准名称',
   BASESALARY           NUMERIC(8,0) COMMENT '基本岗位薪资',
   SALARYLEVELID        VARCHAR(50) COMMENT '薪资等级ID',
   POSTSALARY           NUMERIC(8,0) COMMENT '岗位工资',
   SECURITYALLOWANCE    NUMERIC(8,0) COMMENT '保密津贴',
   HOUSINGALLOWANCE     NUMERIC(8,0) COMMENT '住房津贴',
   AREADIFALLOWANCE     NUMERIC(8,0) COMMENT '地区差异补贴',
   FOODALLOWANCE        NUMERIC(8,0) COMMENT '餐费补贴',
   OTHERADDDEDUCT       NUMERIC(8,0) COMMENT '其他加扣款',
   OTHERADDDEDUCTDESC   VARCHAR(200) COMMENT '加扣款说明',
   HOUSINGALLOWANCEDEDUCT NUMERIC(8,0) COMMENT '住房津贴扣款',
   PERSONALSIRATIO      NUMERIC(8,0) COMMENT '个人社保比',
   PERSONALINCOMERATIO  NUMERIC(8,0) COMMENT '个人所得税比',
   OTHERSUBJOIN         NUMERIC(8,0) COMMENT '其它代扣款',
   OTHERSUBJOINDESC     VARCHAR(50) COMMENT '代扣说明',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   ORDERNUM             NUMERIC(8,0) COMMENT '排序号',
   PRIMARY KEY (SALARYSTANDARDID)
);

ALTER TABLE T_HR_SALARYSTANDARD COMMENT '薪资标准';

/*==============================================================*/
/* Index: IDX_SALAIDID                                          */
/*==============================================================*/
CREATE INDEX IDX_SALAIDID ON T_HR_SALARYSTANDARD
(
   SALARYLEVELID,
   SALARYSOLUTIONID
);

/*==============================================================*/
/* Table: T_HR_SALARYSTANDARDITEM                               */
/*==============================================================*/
CREATE TABLE T_HR_SALARYSTANDARDITEM
(
   STANDRECORDITEMID    VARCHAR(50) NOT NULL COMMENT '薪资标准薪资项ID',
   SALARYSTANDARDID     VARCHAR(50) COMMENT '薪资标准ID',
   SALARYITEMID         VARCHAR(50) COMMENT '薪资项ID',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   ORDERNUMBER          NUMERIC(8,0) COMMENT '排序号',
   SUM                  VARCHAR(2000) COMMENT '金额',
   PRIMARY KEY (STANDRECORDITEMID)
);

ALTER TABLE T_HR_SALARYSTANDARDITEM COMMENT '薪资标准薪资项(#)';

/*==============================================================*/
/* Table: T_HR_SALARYSYSTEM                                     */
/*==============================================================*/
CREATE TABLE T_HR_SALARYSYSTEM
(
   SALARYSYSTEMID       VARCHAR(50) NOT NULL COMMENT '薪酬体系表ID',
   SALARYSYSTEMNAME     VARCHAR(200) COMMENT '薪酬体系表名',
   CHECKSTATE           VARCHAR(1) COMMENT '审核状态',
   EDITSTATE            VARCHAR(1) COMMENT '编辑状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   STARTSALARYLEVEL     NUMERIC(8,0) COMMENT '开始薪资级别',
   ENDSALARYLEVEL       NUMERIC(8,0) COMMENT '结束薪资级别',
   PRIMARY KEY (SALARYSYSTEMID)
);

ALTER TABLE T_HR_SALARYSYSTEM COMMENT '薪酬体系表';

/*==============================================================*/
/* Table: T_HR_SALARYTAXES                                      */
/*==============================================================*/
CREATE TABLE T_HR_SALARYTAXES
(
   SALARYTAXESID        VARCHAR(50) NOT NULL COMMENT '薪资税率ID',
   SALARYSOLUTIONID     VARCHAR(50) COMMENT '薪资方案ID',
   TAXESSUM             NUMERIC(8,0) COMMENT '税级金额',
   TAXESRATE            NUMERIC(8,0) COMMENT '税级税率',
   CALCULATEDEDUCT      NUMERIC(8,0) COMMENT '速算扣除',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   MINITAXESSUM         NUMERIC(8,0) COMMENT '最小税级金额',
   PRIMARY KEY (SALARYTAXESID)
);

ALTER TABLE T_HR_SALARYTAXES COMMENT '薪资税率';

/*==============================================================*/
/* Table: T_HR_SAMPLETABLE                                      */
/*==============================================================*/
CREATE TABLE T_HR_SAMPLETABLE
(
   SAMPLETABLEID        VARCHAR(50) NOT NULL COMMENT '表名ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (SAMPLETABLEID)
);

ALTER TABLE T_HR_SAMPLETABLE COMMENT '示例表名';

/*==============================================================*/
/* Table: T_HR_SAMPLETABLE2                                     */
/*==============================================================*/
CREATE TABLE T_HR_SAMPLETABLE2
(
   EMPLOYEEID           VARCHAR(50) NOT NULL COMMENT '员工ID',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (EMPLOYEEID)
);

ALTER TABLE T_HR_SAMPLETABLE2 COMMENT '示例表';

/*==============================================================*/
/* Table: T_HR_SCHEDULINGTEMPLATEDETAIL                         */
/*==============================================================*/
CREATE TABLE T_HR_SCHEDULINGTEMPLATEDETAIL
(
   TEMPLATEDETAILID     VARCHAR(50) NOT NULL COMMENT '模板明细ID',
   TEMPLATEMASTERID     VARCHAR(50) COMMENT '排班模板主表ID',
   SHIFTDEFINEID        VARCHAR(50) COMMENT '班次ID',
   SCHEDULINGDATE       VARCHAR(50) COMMENT '考勤日名称',
   SCHEDULINGINDEX      NUMERIC(8,0) COMMENT '考勤日序号',
   REMARK               VARCHAR(2000) COMMENT '备注',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   PRIMARY KEY (TEMPLATEDETAILID)
);

ALTER TABLE T_HR_SCHEDULINGTEMPLATEDETAIL COMMENT '排班模板明细';

/*==============================================================*/
/* Table: T_HR_SCHEDULINGTEMPLATEMASTER                         */
/*==============================================================*/
CREATE TABLE T_HR_SCHEDULINGTEMPLATEMASTER
(
   TEMPLATEMASTERID     VARCHAR(50) NOT NULL COMMENT '排班模板主表ID',
   TEMPLATENAME         VARCHAR(50) COMMENT '模板名称',
   SCHEDULINGCIRCLETYPE VARCHAR(50) COMMENT '0月，1周，2天',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   REMARK               VARCHAR(2000) COMMENT '备注',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   PRIMARY KEY (TEMPLATEMASTERID)
);

ALTER TABLE T_HR_SCHEDULINGTEMPLATEMASTER COMMENT '排班模板明细';

/*==============================================================*/
/* Table: T_HR_SHIFTDEFINE                                      */
/*==============================================================*/
CREATE TABLE T_HR_SHIFTDEFINE
(
   SHIFTDEFINEID        VARCHAR(50) NOT NULL COMMENT '班次ID',
   SHIFTNAME            VARCHAR(50) COMMENT '班次名称',
   FIRSTSTARTTIME       VARCHAR(50) COMMENT '第一段上班开始时间',
   FIRSTENDTIME         VARCHAR(50) COMMENT '第一段上班结束时间',
   FIRSTCARDSTARTTIME   VARCHAR(50) COMMENT '第一段上班有效打卡开始时间',
   FIRSTCARDENDTIME     VARCHAR(50) COMMENT '第一段上班有效打卡结束时间',
   NEEDFIRSTCARD        VARCHAR(1) COMMENT '第一段上班是否打卡',
   SECONDSTARTTIME      VARCHAR(50) COMMENT '第二段上班开始时间',
   SECONDENDTIME        VARCHAR(50) COMMENT '第二段上班结束时间',
   NEEDSECONDCARD       VARCHAR(1) COMMENT '第二段上班是否打卡',
   SECONDCARDSTARTTIME  VARCHAR(50) COMMENT '第二段上班有效打卡开始时间',
   SECONDCARDENDTIME    VARCHAR(50) COMMENT '第二段上班有效打卡结束时间',
   THIRDSTARTTIME       VARCHAR(50) COMMENT '第三段上班开始时间',
   THIRDENDTIME         VARCHAR(50) COMMENT '第三段上班结束时间',
   NEEDTHIRDCARD        VARCHAR(1) COMMENT '第三段上班是否打卡',
   THIRDCARDSTARTTIME   VARCHAR(50) COMMENT '第三段上班有效打卡开始时间',
   THIRDCARDENDTIME     VARCHAR(50) COMMENT '第三段上班有效打卡结束时间',
   FOURTHSTARTTIME      VARCHAR(50) COMMENT '第四段上班开始时间',
   FOURTHENDTIME        VARCHAR(50) COMMENT '第四段上班结束时间',
   NEEDFOURTHCARD       VARCHAR(50) COMMENT '第四段上班是否打卡',
   FOURTHCARDSTARTTIME  VARCHAR(50) COMMENT '第四段上班有效打卡开始时间',
   FOURTHCARDENDTIME    VARCHAR(50) COMMENT '第四段上班有效打卡结束时间',
   WORKTIME             NUMERIC(8,0) COMMENT '工作时长',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   NEEDFIRSTOFFCARD     VARCHAR(1) COMMENT '第一段下班是否打卡',
   FIRSTOFFCARDSTARTTIME VARCHAR(50) COMMENT '第一段下班有效打卡开始时间',
   FIRSTOFFCARDENDTIME  VARCHAR(50) COMMENT '第一段下班有效打卡结束时间',
   NEEDSECONDOFFCARD    VARCHAR(1) COMMENT '第二段下班是否打卡',
   SECONDOFFCARDSTARTTIME VARCHAR(50) COMMENT '第二段下班有效打卡开始时间',
   SECONDOFFCARDENDTIME VARCHAR(50) COMMENT '第二段下班有效打卡结束时间',
   NEEDTHIRDOFFCARD     VARCHAR(1) COMMENT '第三段下班是否打卡',
   THIRDOFFCARDENDTIME  VARCHAR(50) COMMENT '第三段下班有效打卡结束时间',
   THIRDOFFCARDSTARTTIME VARCHAR(50) COMMENT '第三段下班有效打卡开始时间',
   NEEDFOURTHOFFCARD    VARCHAR(50) COMMENT '第四段下班是否打卡',
   FOURTHOFFCARDENDTIME VARCHAR(50) COMMENT '第四段下班有效打卡结束时间',
   FOURTHOFFCARDSTARTTIME VARCHAR(50) COMMENT '第四段下班有效打卡开始时间',
   PRIMARY KEY (SHIFTDEFINEID)
);

ALTER TABLE T_HR_SHIFTDEFINE COMMENT '班次定义';

/*==============================================================*/
/* Table: T_HR_SPOTCHECKGROUP                                   */
/*==============================================================*/
CREATE TABLE T_HR_SPOTCHECKGROUP
(
   SPOTCHECKGROUPID     VARCHAR(50) NOT NULL,
   SPOTCHECKGROUPNAME   VARCHAR(50),
   COMPANYID            VARCHAR(50),
   COMPANYNAME          VARCHAR(50),
   DEPARTMENTID         VARCHAR(50),
   DEPARTMENTNAME       VARCHAR(50),
   POSTID               VARCHAR(50),
   POSTNAME             VARCHAR(50),
   POSTLEVEL            VARCHAR(1),
   EMPLOYEEID           VARCHAR(50),
   EMPLOYEENAME         VARCHAR(50),
   REMARK               VARCHAR(2000),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME,
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (SPOTCHECKGROUPID)
);

ALTER TABLE T_HR_SPOTCHECKGROUP COMMENT '抽查组定义';

/*==============================================================*/
/* Table: T_HR_SPOTCHECKER                                      */
/*==============================================================*/
CREATE TABLE T_HR_SPOTCHECKER
(
   SPOTCHECKERID        VARCHAR(50) NOT NULL,
   SPOTCHECKGROUPID     VARCHAR(50),
   EMPLOYEEID           VARCHAR(50),
   EMPLOYEENAME         VARCHAR(50),
   COMPANYID            VARCHAR(50),
   COMPANYNAME          VARCHAR(50),
   DEPARTMENTID         VARCHAR(50),
   DEPARTMENTNAME       VARCHAR(50),
   POSTID               VARCHAR(50),
   POSTNAME             VARCHAR(50),
   POSTLEVEL            VARCHAR(50),
   REMARK               VARCHAR(2000),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME,
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (SPOTCHECKERID)
);

ALTER TABLE T_HR_SPOTCHECKER COMMENT '抽查人员';

/*==============================================================*/
/* Table: T_HR_STANDREWARDARCHIVE                               */
/*==============================================================*/
CREATE TABLE T_HR_STANDREWARDARCHIVE
(
   STANDREWARDARCHIVEID VARCHAR(50) NOT NULL COMMENT '薪资标准绩效奖ID',
   SALARYARCHIVEID      VARCHAR(50) COMMENT '薪资档案ID',
   PERFORMANCEREWARDID  VARCHAR(50) COMMENT '绩效奖金ID',
   SUM                  NUMERIC(8,0) COMMENT '金额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (STANDREWARDARCHIVEID)
);

ALTER TABLE T_HR_STANDREWARDARCHIVE COMMENT '薪资标准绩效奖档案';

/*==============================================================*/
/* Table: T_HR_STANDREWARDARCHIVEHIS                            */
/*==============================================================*/
CREATE TABLE T_HR_STANDREWARDARCHIVEHIS
(
   STANDREWARDARCHIVEID VARCHAR(50) NOT NULL COMMENT '薪资标准绩效奖ID',
   SALARYARCHIVEID      VARCHAR(50) COMMENT '薪资档案ID',
   PERFORMANCEREWARDID  VARCHAR(50) COMMENT '绩效奖金ID',
   SUM                  NUMERIC(8,0) COMMENT '金额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (STANDREWARDARCHIVEID)
);

ALTER TABLE T_HR_STANDREWARDARCHIVEHIS COMMENT '薪资标准绩效奖档案历史';

/*==============================================================*/
/* Table: T_HR_STANDARDPERFORMANCEREWARD                        */
/*==============================================================*/
CREATE TABLE T_HR_STANDARDPERFORMANCEREWARD
(
   STANDARDPERFORMANCEREWARDID VARCHAR(50) NOT NULL COMMENT '薪资标准绩效奖ID',
   SALARYSTANDARDID     VARCHAR(50) COMMENT '薪资标准ID',
   PERFORMANCEREWARDSETID VARCHAR(50) COMMENT '绩效奖金ID',
   SUM                  NUMERIC(8,0) COMMENT '金额',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (STANDARDPERFORMANCEREWARDID)
);

ALTER TABLE T_HR_STANDARDPERFORMANCEREWARD COMMENT '薪资标准绩效奖';

/*==============================================================*/
/* Table: T_HR_SYSTEMSETTING                                    */
/*==============================================================*/
CREATE TABLE T_HR_SYSTEMSETTING
(
   SYSTEMSETTINGID      VARCHAR(50) NOT NULL COMMENT '系统参数ID',
   MODELTYPE            VARCHAR(50) COMMENT '模块类型:0 全局参数;1组织架构参数,2人事管理参数,3考勤管理参数,4薪资管理参数,5绩效管理参数',
   MODELVALUE           VARCHAR(50) COMMENT '模块类型值',
   PARAMETERNAME        VARCHAR(50) COMMENT '参数名称',
   PARAMETERVALUE       VARCHAR(50) COMMENT '参数值',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (SYSTEMSETTINGID)
);

ALTER TABLE T_HR_SYSTEMSETTING COMMENT '系统参数表';

/*==============================================================*/
/* Table: T_HR_SYSTEMSETTING2                                   */
/*==============================================================*/
CREATE TABLE T_HR_SYSTEMSETTING2
(
   SYSTEMSETTINGID      VARCHAR(50) NOT NULL,
   MODELNAME            VARCHAR(36) COMMENT '模块名：
            人事模块
            考勤模块
            薪资模块',
   MODELCODE            VARCHAR(36),
   PARAMETERNAME        VARCHAR(50),
   PARAMETERCODE        VARCHAR(50),
   PARAMETERVALUE       VARCHAR(100),
   PARAMETERDESC        VARCHAR(50),
   REMARK               VARCHAR(2000),
   UPDATEDATE           DATETIME ,
   UPDATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME ,
   CREATEUSERID         VARCHAR(50),
   PRIMARY KEY (SYSTEMSETTINGID)
);

ALTER TABLE T_HR_SYSTEMSETTING2 COMMENT '系统设置表';

/*==============================================================*/
/* Table: T_HR_VACATIONSET                                      */
/*==============================================================*/
CREATE TABLE T_HR_VACATIONSET
(
   VACATIONID           VARCHAR(50) NOT NULL COMMENT '工作日历ID',
   VACATIONNAME         VARCHAR(50) COMMENT '工作日历名称',
   ASSIGNEDOBJECTTYPE   VARCHAR(50) COMMENT '分配对像类别',
   ASSIGNEDOBJECTID     VARCHAR(2000) COMMENT '分配对像ID',
   VACATIONYEAR         VARCHAR(50) COMMENT '所在年份',
   COUNTYTYPE           VARCHAR(1) COMMENT '国别',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   PRIMARY KEY (VACATIONID)
);

ALTER TABLE T_HR_VACATIONSET COMMENT '工作日历';

/*==============================================================*/
/* Table: T_OA_ACCIDENTRECORD                                   */
/*==============================================================*/
CREATE TABLE T_OA_ACCIDENTRECORD
(
   ACCIDENTRECORDID     VARCHAR(50) NOT NULL,
   ASSETID              VARCHAR(50) NOT NULL,
   CONTENT              VARCHAR(200) NOT NULL,
   ACCIDENTDATE         DATETIME NOT NULL,
   FLAG                 VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:未处理,1:已处理',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (ACCIDENTRECORDID)
);

ALTER TABLE T_OA_ACCIDENTRECORD COMMENT 'T_OA_ACCIDENTRECORD';

/*==============================================================*/
/* Table: T_OA_AGENTDATESET                                     */
/*==============================================================*/
CREATE TABLE T_OA_AGENTDATESET
(
   AGENTDATESETID       VARCHAR(50) NOT NULL,
   MODELCODE            VARCHAR(50) NOT NULL,
   USERID               VARCHAR(50) NOT NULL,
   EFFECTIVEDATE        DATETIME NOT NULL,
   PLANEXPIRATIONDATE   DATETIME NOT NULL,
   EXPIRATIONDATE       DATETIME,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (AGENTDATESETID)
);

ALTER TABLE T_OA_AGENTDATESET COMMENT '使用代理时效设置表';

/*==============================================================*/
/* Table: T_OA_AGENTSET                                         */
/*==============================================================*/
CREATE TABLE T_OA_AGENTSET
(
   AGENTSETID           VARCHAR(50) NOT NULL,
   SYSCODE              VARCHAR(50) NOT NULL,
   MODELCODE            VARCHAR(50) NOT NULL,
   USERID               VARCHAR(50) NOT NULL,
   AGENTPOSTID          VARCHAR(50) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (AGENTSETID)
);

ALTER TABLE T_OA_AGENTSET COMMENT '代理设置';

/*==============================================================*/
/* Table: T_OA_APPROVALINFO                                     */
/*==============================================================*/
CREATE TABLE T_OA_APPROVALINFO
(
   APPROVALID           VARCHAR(50) NOT NULL,
   APPROVALCODE         VARCHAR(200) NOT NULL,
   APPROVALTITLE        VARCHAR(200) NOT NULL,
   TEL                  VARCHAR(50) NOT NULL,
   CONTENT              LONGBLOB NOT NULL,
   ISCHARGE             VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：无费用，1：有费用',
   CHARGEMONEY          NUMERIC(8,0) DEFAULT 0,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；1：审核中；2：审核通过；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   TYPEAPPROVAL         VARCHAR(50),
   TYPEAPPROVALONE      VARCHAR(50),
   TYPEAPPROVALTWO      VARCHAR(50),
   TYPEAPPROVALTHREE    VARCHAR(50),
   ISPERSONFLOW         VARCHAR(50),
   ISSHOW               VARCHAR(50) DEFAULT '1' COMMENT '是否显示：0 不显示  1 显示',
   ISMULTPERSON         VARCHAR(50) DEFAULT '0' COMMENT '是否选多个审核人：0 一个  1 选多个审核人',
   PRIMARY KEY (APPROVALID)
);

ALTER TABLE T_OA_APPROVALINFO COMMENT 'T_OA_APPROVALINFO';

/*==============================================================*/
/* Index: IDX_CREATEUSERID                                      */
/*==============================================================*/
CREATE INDEX IDX_CREATEUSERID ON T_OA_APPROVALINFO
(
   CREATEUSERID
);

/*==============================================================*/
/* Table: T_OA_APPROVALINFOTEMPLET                              */
/*==============================================================*/
CREATE TABLE T_OA_APPROVALINFOTEMPLET
(
   APPROVALID           VARCHAR(50) NOT NULL,
   APPROVALTITLE        VARCHAR(200) NOT NULL,
   TEL                  VARCHAR(50) NOT NULL,
   CONTENT              LONGBLOB NOT NULL,
   ISCHARGE             VARCHAR(1) NOT NULL DEFAULT '0',
   CHARGEMONEY          NUMERIC(8,0) DEFAULT 0,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   TYPEAPPROVAL         VARCHAR(50),
   TYPEAPPROVALONE      VARCHAR(50),
   TYPEAPPROVALTWO      VARCHAR(50),
   TYPEAPPROVALTHREE    VARCHAR(50),
   PRIMARY KEY (APPROVALID)
);

ALTER TABLE T_OA_APPROVALINFOTEMPLET COMMENT 'T_OA_APPROVALINFOTEMPLET';

/*==============================================================*/
/* Table: T_OA_APPROVALTYPESET                                  */
/*==============================================================*/
CREATE TABLE T_OA_APPROVALTYPESET
(
   TYPESETID            VARCHAR(50) NOT NULL COMMENT '类型设置ID',
   ORGANIZATIONID       VARCHAR(50) COMMENT '组织架构ID',
   ORGANIZATIONTYPE     VARCHAR(50) COMMENT '组织架构类型',
   TYPEAPPROVAL         VARCHAR(2000) COMMENT '事项审批类型值',
   OWNERCOMPANYID       VARCHAR(50) NOT NULL COMMENT '所属公司',
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL COMMENT '所属部门',
   OWNERPOSTID          VARCHAR(50) NOT NULL COMMENT '所属岗位',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人',
   CREATEUSERNAME       VARCHAR(50) NOT NULL COMMENT '创建人名',
   CREATECOMPANYID      VARCHAR(50) NOT NULL COMMENT '创建公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL COMMENT '创建部门ID',
   CREATEPOSTID         VARCHAR(50) NOT NULL COMMENT '创建岗位ID',
   CREATEDATE           DATETIME NOT NULL  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改人名',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (TYPESETID)
);

ALTER TABLE T_OA_APPROVALTYPESET COMMENT '事项审批类型设置';

/*==============================================================*/
/* Table: T_OA_ARCHIVES                                         */
/*==============================================================*/
CREATE TABLE T_OA_ARCHIVES
(
   ARCHIVESID           VARCHAR(50) NOT NULL,
   COMPANYID            VARCHAR(50) NOT NULL,
   ARCHIVESTITLE        VARCHAR(200) NOT NULL,
   CONTENT              LONGBLOB NOT NULL,
   SOURCEFLAG           VARCHAR(1) NOT NULL DEFAULT '1' COMMENT '0:自动导入，1：输入,有原件，可借阅',
   RECORDTYPE           VARCHAR(50) NOT NULL COMMENT 'SENDDOC代表是公司发文等',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (ARCHIVESID)
);

ALTER TABLE T_OA_ARCHIVES COMMENT '如有附件上传到OA_FILEUPLOAD_T,模块名取ARCHIVES';

/*==============================================================*/
/* Table: T_OA_AREAALLOWANCE                                    */
/*==============================================================*/
CREATE TABLE T_OA_AREAALLOWANCE
(
   AREAALLOWANCEID      VARCHAR(50) NOT NULL COMMENT '地区补贴ID',
   AREADIFFERENCEID     VARCHAR(50) COMMENT '地区分类ID',
   POSTLEVEL            VARCHAR(50) COMMENT '岗位等级',
   ACCOMMODATION        NUMERIC(8,0) COMMENT '住宿补贴费用',
   TRANSPORTATIONSUBSIDIES NUMERIC(8,0) COMMENT '交通补贴费用',
   MEALSUBSIDIES        NUMERIC(8,0) COMMENT '餐费补贴',
   OVERSEASSUBSIDIES    NUMERIC(8,0) COMMENT '驻外补贴标准',
   TRAFFICMEALALLOWANCE NUMERIC(8,0),
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   TRAVELSOLUTIONSID    VARCHAR(50) COMMENT '方案id',
   PRIMARY KEY (AREAALLOWANCEID)
);

ALTER TABLE T_OA_AREAALLOWANCE COMMENT '地区差异补贴';

/*==============================================================*/
/* Table: T_OA_AREACITY                                         */
/*==============================================================*/
CREATE TABLE T_OA_AREACITY
(
   AREACITYID           VARCHAR(50) NOT NULL COMMENT '地区分类城市ID',
   AREADIFFERENCEID     VARCHAR(50) COMMENT '地区分类ID',
   CITY                 VARCHAR(10) COMMENT '所在地城市，系统字典中定义',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (AREACITYID)
);

ALTER TABLE T_OA_AREACITY COMMENT '地区分类城市';

/*==============================================================*/
/* Table: T_OA_AREADIFFERENCE                                   */
/*==============================================================*/
CREATE TABLE T_OA_AREADIFFERENCE
(
   AREADIFFERENCEID     VARCHAR(50) NOT NULL COMMENT '地区分类ID',
   AREACATEGORY         VARCHAR(2000) COMMENT '地区分类名',
   AREAINDEX            NUMERIC(8,0) COMMENT '地区分类序号',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEUSERNAME       VARCHAR(50) NOT NULL COMMENT '创建人名',
   CREATECOMPANYID      VARCHAR(50) NOT NULL COMMENT '创建公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL COMMENT '创建部门ID',
   CREATEPOSTID         VARCHAR(50) NOT NULL COMMENT '创建岗位ID',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改人名',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   TRAVELSOLUTIONSID    VARCHAR(50) COMMENT '方案id',
   PRIMARY KEY (AREADIFFERENCEID)
);

ALTER TABLE T_OA_AREADIFFERENCE COMMENT '地区差异分类表';

/*==============================================================*/
/* Table: T_OA_BUSINESSREPORT                                   */
/*==============================================================*/
CREATE TABLE T_OA_BUSINESSREPORT
(
   BUSINESSREPORTID     VARCHAR(50) NOT NULL COMMENT '出差报告ID',
   BUSINESSTRIPID       VARCHAR(50) COMMENT '出差申请ID',
   TEL                  VARCHAR(50) COMMENT '联系电话',
   CONTENT              VARCHAR(2000) COMMENT '出差报告',
   CHECKSTATE           VARCHAR(1) DEFAULT '0' COMMENT '0：未提交；1：审核通过；2：审核中；3：审核未通过',
   CHARGEMONEY          NUMERIC(8,0) DEFAULT 0 COMMENT '报销总额',
   REMARKS              VARCHAR(200) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '出差人',
   OWNERNAME            VARCHAR(50) COMMENT '出差人名',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人名',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建部门ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建岗位ID',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改人名',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (BUSINESSREPORTID)
);

ALTER TABLE T_OA_BUSINESSREPORT COMMENT '出差报告';

/*==============================================================*/
/* Table: T_OA_BUSINESSREPORTDETAIL                             */
/*==============================================================*/
CREATE TABLE T_OA_BUSINESSREPORTDETAIL
(
   BUSINESSREPORTDETAILID VARCHAR(50) NOT NULL COMMENT '报告子表ID',
   BUSINESSREPORTID     VARCHAR(50) NOT NULL COMMENT '出差报告ID',
   STARTDATE            DATETIME COMMENT '开始时间',
   ENDDATE              DATETIME COMMENT '结束时间',
   BUSINESSDAYS         VARCHAR(50) COMMENT '出差天数',
   DEPCITY              VARCHAR(50) NOT NULL COMMENT '通过SYS_TYPEGROUP_T获取地区定义名',
   DESTCITY             VARCHAR(50) NOT NULL COMMENT '通过SYS_TYPEGROUP_T获取地区定义名',
   PRIVATEAFFAIR        VARCHAR(1) COMMENT '私事',
   TYPEOFTRAVELTOOLS    VARCHAR(50) COMMENT '乘坐的交通工具类型',
   TAKETHETOOLLEVEL     VARCHAR(50) COMMENT '交通工具的级别(例如：飞机头等舱)',
   GOOUTTOMEET          VARCHAR(1) COMMENT '内部会议或内部培训',
   COMPANYCAR           VARCHAR(1) COMMENT '公司派车',
   PRIMARY KEY (BUSINESSREPORTDETAILID)
);

ALTER TABLE T_OA_BUSINESSREPORTDETAIL COMMENT '出差报告子表';

/*==============================================================*/
/* Table: T_OA_BUSINESSTRIP                                     */
/*==============================================================*/
CREATE TABLE T_OA_BUSINESSTRIP
(
   BUSINESSTRIPID       VARCHAR(50) NOT NULL COMMENT '出差申请ID',
   TEL                  VARCHAR(50) COMMENT '紧急联系电话',
   ISAGENT              VARCHAR(1) DEFAULT '0' COMMENT '0:不启用，1:启用',
   ISCHARGE             VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：无费用，1：有费用',
   CHARGEMONEY          NUMERIC(8,0) DEFAULT 0 COMMENT '借款总额',
   CHECKSTATE           VARCHAR(1) COMMENT '0：未提交；1：审核中；2：审核通过；3：审核未通过',
   CONTENT              VARCHAR(2000) COMMENT '出差事由',
   REMARKS              VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '出差人ID',
   OWNERNAME            VARCHAR(50) COMMENT '出差人名称',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人名',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建部门ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建岗位ID',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改人名',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   STARTDATE            DATETIME COMMENT '开始时间',
   ENDDATE              DATETIME COMMENT '结束时间',
   DEPCITY              VARCHAR(50) COMMENT '出发城市',
   DESTCITY             VARCHAR(50) COMMENT '目的城市',
   OWNERPOSTNAME        VARCHAR(100),
   OWNERDEPARTMENTNAME  VARCHAR(100),
   OWNERCOMPANYNAME     VARCHAR(100),
   POSTLEVEL            VARCHAR(10),
   STARTCITYNAME        VARCHAR(10),
   ENDCITYNAME          VARCHAR(10),
   ISALTERTRAVE         VARCHAR(10),
   ALTERDETAILBASE      VARCHAR(10) COMMENT '第一次终审的出差详情',
   ALTERDETAILBEFORE    VARCHAR(2000) COMMENT '修改行程之前出差详情',
   ALTERDETAILATFER     VARCHAR(10) COMMENT '修改行程之后出差详情',
   ISFROMWP             VARCHAR(10) COMMENT '来自工作计划',
   PRIMARY KEY (BUSINESSTRIPID)
);

ALTER TABLE T_OA_BUSINESSTRIP COMMENT '出差申请';

/*==============================================================*/
/* Table: T_OA_BUSINESSTRIPDETAIL                               */
/*==============================================================*/
CREATE TABLE T_OA_BUSINESSTRIPDETAIL
(
   BUSINESSTRIPDETAILID VARCHAR(50) NOT NULL COMMENT '申请子表ID',
   BUSINESSTRIPID       VARCHAR(50) NOT NULL COMMENT '出差申请ID',
   STARTDATE            DATETIME COMMENT '开始时间',
   ENDDATE              DATETIME COMMENT '结束时间',
   DEPCITY              VARCHAR(50) NOT NULL COMMENT '通过SYS_TYPEGROUP_T获取地区定义名',
   DESTCITY             VARCHAR(50) NOT NULL COMMENT '通过SYS_TYPEGROUP_T获取地区定义名',
   PRIVATEAFFAIR        VARCHAR(1) COMMENT '私事',
   TYPEOFTRAVELTOOLS    VARCHAR(50) COMMENT '乘坐的交通工具类型',
   TAKETHETOOLLEVEL     VARCHAR(50) COMMENT '交通工具的级别(例如：飞机头等舱)',
   GOOUTTOMEET          VARCHAR(1),
   COMPANYCAR           VARCHAR(1) COMMENT '公司派车',
   BUSINESSDAYS         VARCHAR(50) COMMENT '出差天数',
   STARTCITYNAME        VARCHAR(2000),
   ENDCITYNAME          VARCHAR(2000),
   PRIMARY KEY (BUSINESSTRIPDETAILID)
);

ALTER TABLE T_OA_BUSINESSTRIPDETAIL COMMENT '出差申请子表';

/*==============================================================*/
/* Table: T_OA_BUSINESSTRIPDETAIL_BAK                           */
/*==============================================================*/
CREATE TABLE T_OA_BUSINESSTRIPDETAIL_BAK
(
   BUSINESSTRIPDETAILID VARCHAR(50) NOT NULL,
   BUSINESSTRIPID       VARCHAR(50) NOT NULL,
   STARTDATE            DATETIME,
   ENDDATE              DATETIME,
   DEPCITY              VARCHAR(50) NOT NULL,
   DESTCITY             VARCHAR(50) NOT NULL,
   PRIVATEAFFAIR        VARCHAR(1),
   TYPEOFTRAVELTOOLS    VARCHAR(50),
   TAKETHETOOLLEVEL     VARCHAR(50),
   GOOUTTOMEET          VARCHAR(1),
   COMPANYCAR           VARCHAR(1),
   BUSINESSDAYS         VARCHAR(50),
   STARTCITYNAME        VARCHAR(2000),
   ENDCITYNAME          VARCHAR(2000)
);

ALTER TABLE T_OA_BUSINESSTRIPDETAIL_BAK COMMENT 'T_OA_BUSINESSTRIPDETAIL_BAK';

/*==============================================================*/
/* Table: T_OA_BUSINESSTRIP_BAK                                 */
/*==============================================================*/
CREATE TABLE T_OA_BUSINESSTRIP_BAK
(
   BUSINESSTRIPID       VARCHAR(50) NOT NULL,
   TEL                  VARCHAR(50),
   ISAGENT              VARCHAR(1),
   CHARGEMONEY          NUMERIC(8,0),
   CHECKSTATE           VARCHAR(1),
   CONTENT              VARCHAR(2000),
   REMARKS              VARCHAR(200),
   OWNERID              VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEUSERNAME       VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDATE           DATETIME,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   STARTDATE            DATETIME,
   ENDDATE              DATETIME,
   DEPCITY              VARCHAR(50),
   DESTCITY             VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(2000),
   OWNERDEPARTMENTNAME  VARCHAR(2000),
   OWNERCOMPANYNAME     VARCHAR(2000),
   POSTLEVEL            VARCHAR(2000),
   STARTCITYNAME        VARCHAR(2000),
   ENDCITYNAME          VARCHAR(2000),
   ISALTERTRAVE         VARCHAR(2000)
);

ALTER TABLE T_OA_BUSINESSTRIP_BAK COMMENT 'T_OA_BUSINESSTRIP_BAK';

/*==============================================================*/
/* Table: T_OA_CALENDAR                                         */
/*==============================================================*/
CREATE TABLE T_OA_CALENDAR
(
   CALENDARID           VARCHAR(50) NOT NULL,
   TITLE                VARCHAR(50) NOT NULL,
   CONTENT              VARCHAR(1000) NOT NULL,
   REMINDERRMODEL       VARCHAR(50) NOT NULL,
   PLANTIME             DATETIME NOT NULL,
   REPARTREMINDER       VARCHAR(50) NOT NULL COMMENT 'NOTHING：不重复，DAY：每天，WEEK：按周，MONTH：按月，YEAR：按年',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (CALENDARID)
);

ALTER TABLE T_OA_CALENDAR COMMENT 'T_OA_CALENDAR';

/*==============================================================*/
/* Table: T_OA_CANTAKETHEPLANELINE                              */
/*==============================================================*/
CREATE TABLE T_OA_CANTAKETHEPLANELINE
(
   CANTAKETHEPLANELINEID VARCHAR(50) NOT NULL COMMENT 'ID',
   TRAVELSOLUTIONSID    VARCHAR(50) COMMENT '方案id',
   REGIONAL             VARCHAR(50) COMMENT '例如：南区，在字典中定义',
   DEPCITY              VARCHAR(50) NOT NULL COMMENT '通过SYS_TYPEGROUP_T获取地区定义名',
   DESTCITY             VARCHAR(50) NOT NULL COMMENT '通过SYS_TYPEGROUP_T获取地区定义名',
   LANDTIME             VARCHAR(20) COMMENT '陆路所需时间',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人',
   CREATEDATE           DATETIME NOT NULL  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (CANTAKETHEPLANELINEID)
);

ALTER TABLE T_OA_CANTAKETHEPLANELINE COMMENT '出差可乘坐飞机线路设置';

/*==============================================================*/
/* Table: T_OA_CONSERVATION                                     */
/*==============================================================*/
CREATE TABLE T_OA_CONSERVATION
(
   CONSERVATIONID       VARCHAR(50) NOT NULL,
   ASSETID              VARCHAR(50) NOT NULL,
   CONSERVATYPE         VARCHAR(50) NOT NULL COMMENT '从系统字典表中获取',
   CONTENT              VARCHAR(200) NOT NULL,
   TEL                  VARCHAR(50) NOT NULL,
   REPAIRDATE           DATETIME NOT NULL,
   RETRIEVEDATE         DATETIME NOT NULL,
   CONSERVATIONCOMPANY  VARCHAR(200) NOT NULL,
   CONSERVATIONRANGE    NUMERIC(8,0),
   REMARK               VARCHAR(200),
   ISCHARGE             VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：无费用，1：有费用',
   CHARGEMONEY          NUMERIC(8,0) DEFAULT 0,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；1：审核通过；2：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (CONSERVATIONID)
);

ALTER TABLE T_OA_CONSERVATION COMMENT 'T_OA_CONSERVATION';

/*==============================================================*/
/* Table: T_OA_CONSERVATIONRECORD                               */
/*==============================================================*/
CREATE TABLE T_OA_CONSERVATIONRECORD
(
   CONSERVATIONRECORDID VARCHAR(50) NOT NULL,
   CONSERVATIONID       VARCHAR(50) NOT NULL,
   CONSERVATYPE         VARCHAR(50) NOT NULL COMMENT '从系统字典表中获取',
   CONTENT              VARCHAR(200) NOT NULL,
   TEL                  VARCHAR(50) NOT NULL,
   REPAIRDATE           DATETIME NOT NULL,
   RETRIEVEDATE         DATETIME NOT NULL,
   CONSERVATIONRANGE    NUMERIC(8,0),
   REMARK               VARCHAR(200),
   ISCHARGE             VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：无费用，1：有费用',
   CHARGEMONEY          NUMERIC(8,0) DEFAULT 0,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；1：审核通过；2：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (CONSERVATIONRECORDID)
);

ALTER TABLE T_OA_CONSERVATIONRECORD COMMENT 'T_OA_CONSERVATIONRECORD';

/*==============================================================*/
/* Table: T_OA_CONTRACTAPP                                      */
/*==============================================================*/
CREATE TABLE T_OA_CONTRACTAPP
(
   CONTRACTAPPID        VARCHAR(50) NOT NULL,
   CONTRACTCODE         VARCHAR(50) NOT NULL,
   CONTRACTTYPEID       VARCHAR(50) NOT NULL,
   CONTRACTLEVEL        VARCHAR(50) NOT NULL,
   PARTYA               VARCHAR(50) NOT NULL,
   PARTYB               VARCHAR(50) NOT NULL,
   STARTDATE            DATETIME NOT NULL,
   ENDDATE              DATETIME NOT NULL COMMENT '合同到期时间',
   CONTRACTFLAG         VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:商务合同，1：员工合同',
   EXPIRATIONREMINDER   NUMERIC(8,0) NOT NULL DEFAULT 0 COMMENT '0:商务合同，1：员工合同',
   CONTRACTTITLE        VARCHAR(100) NOT NULL,
   CONTENT              LONGBLOB NOT NULL,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；1：审核通过；2：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (CONTRACTAPPID)
);

ALTER TABLE T_OA_CONTRACTAPP COMMENT 'T_OA_CONTRACTAPP';

/*==============================================================*/
/* Table: T_OA_CONTRACTPRINT                                    */
/*==============================================================*/
CREATE TABLE T_OA_CONTRACTPRINT
(
   CONTRACTPRINTID      VARCHAR(50) NOT NULL,
   CONTRACTAPPID        VARCHAR(50) NOT NULL,
   NUM                  NUMERIC(8,0) NOT NULL,
   SIGNDATE             DATETIME,
   ISUPLOAD             VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:未上传，1:已是传',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (CONTRACTPRINTID)
);

ALTER TABLE T_OA_CONTRACTPRINT COMMENT 'T_OA_CONTRACTPRINT';

/*==============================================================*/
/* Table: T_OA_CONTRACTTEMPLATE                                 */
/*==============================================================*/
CREATE TABLE T_OA_CONTRACTTEMPLATE
(
   CONTRACTTEMPLATEID   VARCHAR(50) NOT NULL,
   CONTRACTTEMPLATENAME VARCHAR(50) NOT NULL,
   CONTRACTTYPEID       VARCHAR(50) NOT NULL,
   CONTRACTLEVEL        VARCHAR(50) NOT NULL,
   CONTRACTTITLE        VARCHAR(100) NOT NULL,
   CONTENT              LONGBLOB NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (CONTRACTTEMPLATEID)
);

ALTER TABLE T_OA_CONTRACTTEMPLATE COMMENT 'T_OA_CONTRACTTEMPLATE';

/*==============================================================*/
/* Table: T_OA_CONTRACTTYPE                                     */
/*==============================================================*/
CREATE TABLE T_OA_CONTRACTTYPE
(
   CONTRACTTYPEID       VARCHAR(50) NOT NULL,
   CONTRACTTYPE         VARCHAR(50),
   CONTRACTLEVEL        VARCHAR(50) NOT NULL,
   CONTENT              VARCHAR(600) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (CONTRACTTYPEID)
);

ALTER TABLE T_OA_CONTRACTTYPE COMMENT 'T_OA_CONTRACTTYPE';

/*==============================================================*/
/* Table: T_OA_CONTRACTVIEW                                     */
/*==============================================================*/
CREATE TABLE T_OA_CONTRACTVIEW
(
   CONTRACTVIEWID       VARCHAR(50) NOT NULL,
   CONTRACTPRINTID      VARCHAR(50) NOT NULL,
   TEL                  VARCHAR(50) NOT NULL,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；1：审核通过；2：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (CONTRACTVIEWID)
);

ALTER TABLE T_OA_CONTRACTVIEW COMMENT 'T_OA_CONTRACTVIEW';

/*==============================================================*/
/* Table: T_OA_COSTRECORD                                       */
/*==============================================================*/
CREATE TABLE T_OA_COSTRECORD
(
   COSTRECORDID         VARCHAR(50) NOT NULL,
   ASSETID              VARCHAR(50) NOT NULL,
   MODELNAME            VARCHAR(50),
   FORMID               VARCHAR(50),
   CONSTTYPE            VARCHAR(50) NOT NULL COMMENT '通过字典获取保养类型',
   CONTENT              VARCHAR(50) NOT NULL,
   COST                 NUMERIC(8,0) NOT NULL DEFAULT 0,
   COSTDATE             DATETIME NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (COSTRECORDID)
);

ALTER TABLE T_OA_COSTRECORD COMMENT 'T_OA_COSTRECORD';

/*==============================================================*/
/* Table: T_OA_DISTRIBUTEUSER                                   */
/*==============================================================*/
CREATE TABLE T_OA_DISTRIBUTEUSER
(
   DISTRIBUTEUSERID     VARCHAR(50) NOT NULL,
   MODELNAME            VARCHAR(50) NOT NULL,
   FORMID               VARCHAR(50) NOT NULL,
   VIEWTYPE             VARCHAR(1) NOT NULL DEFAULT '3' COMMENT '1：按公司，2：按部门，3：按用户',
   VIEWER               VARCHAR(50) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   VIEWNAME             VARCHAR(100),
   PRIMARY KEY (MODELNAME, FORMID, VIEWTYPE, VIEWER)
);

ALTER TABLE T_OA_DISTRIBUTEUSER COMMENT 'T_OA_DISTRIBUTEUSER';

/*==============================================================*/
/* Table: T_OA_GIFTAPPLYDETAIL                                  */
/*==============================================================*/
CREATE TABLE T_OA_GIFTAPPLYDETAIL
(
   GIFTAPPLYDETAILID    VARCHAR(50) NOT NULL COMMENT '礼品派送申请单明细ID',
   GIFTAPPLYMASTERID    VARCHAR(50) COMMENT '礼品派送申报单ID',
   SENDERNAME           VARCHAR(50) COMMENT '发件人名称',
   SENDERPHONE          VARCHAR(50) COMMENT '发件人电话',
   SENDUNITNAME         VARCHAR(50) COMMENT '发件单位名称',
   RECIPIENTID          VARCHAR(50) COMMENT '收件人ID',
   RECIPIENTNAME        VARCHAR(50) COMMENT '收件人名称',
   RECIPIENTPHONE       VARCHAR(50) COMMENT '收件人电话',
   RECIPIENTUNITID      VARCHAR(50) COMMENT '收件人单位ID',
   RECIPIENTUNITNAME    VARCHAR(50) COMMENT '收件人单位名称',
   ADRESSDETAIL         VARCHAR(200) COMMENT '详细地址',
   PROVINCE             VARCHAR(50) COMMENT '所属省份',
   CITY                 VARCHAR(50) COMMENT '所属市',
   AREA                 VARCHAR(50) COMMENT '区\县',
   COUNT                NUMERIC(8,0) NOT NULL COMMENT '数量',
   SENDREQUIRE          VARCHAR(200) COMMENT '派送要求',
   REMARK               VARCHAR(200) COMMENT '备注',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人ID',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人名称',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) NOT NULL COMMENT '修改人ID',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改人名称',
   UPDATEDATE           DATETIME NOT NULL COMMENT '修改时间',
   ITEMSTATES           VARCHAR(50) COMMENT '条目状态',
   PRIMARY KEY (GIFTAPPLYDETAILID)
);

ALTER TABLE T_OA_GIFTAPPLYDETAIL COMMENT '礼品派送申请单明细';

/*==============================================================*/
/* Table: T_OA_GIFTAPPLYMASTER                                  */
/*==============================================================*/
CREATE TABLE T_OA_GIFTAPPLYMASTER
(
   GIFTAPPLYMASTERID    VARCHAR(50) NOT NULL COMMENT '礼品派送申报单ID',
   GIFTPLANID           VARCHAR(50) COMMENT '礼品派送计划ID',
   GIFTAPPLYMASTERCODE  VARCHAR(50) COMMENT '礼品派送申报单编号',
   SENDTYPE             VARCHAR(50) COMMENT '1:内部员工，　２：客户',
   OWNERCOMPANYID       VARCHAR(50) NOT NULL COMMENT '申报人公司ID',
   OWNERCOMPANYNAME     VARCHAR(50) COMMENT '申报人公司名称',
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL COMMENT '申报人部门ID',
   OWNERDEPARTMENTNAME  VARCHAR(50) COMMENT '申报人部门名称',
   OWNERPOSTID          VARCHAR(50) NOT NULL COMMENT '申报人岗位ID',
   OWNERPOSTNAME        VARCHAR(50) COMMENT '申报人岗位名称',
   OWNERID              VARCHAR(50) NOT NULL COMMENT '申报人ID',
   OWNERNAME            VARCHAR(50) COMMENT '申报人名称',
   CREATECOMPANYID      VARCHAR(50) NOT NULL COMMENT '创建人公司ID',
   CREATECOMPANYNAME    VARCHAR(50) COMMENT '创建人公司名称',
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL COMMENT '创建人部门ID',
   CREATEDEPARTMENTNAME VARCHAR(50) COMMENT '创建人部门名称',
   CREATEPOSTID         VARCHAR(50) NOT NULL COMMENT '创建人岗位ID',
   CREATEPOSTNAME       VARCHAR(50) COMMENT '创建人岗位名称',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人ID',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人名称',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) NOT NULL COMMENT '修改人ID',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改人名称',
   UPDATEDATE           DATETIME NOT NULL COMMENT '修改时间',
   REMARK               VARCHAR(2000) COMMENT '备注',
   EDITSTATES           VARCHAR(50) NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled',
   CHECKSTATES          VARCHAR(50) NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved',
   PRIMARY KEY (GIFTAPPLYMASTERID)
);

ALTER TABLE T_OA_GIFTAPPLYMASTER COMMENT '礼品派送申报单';

/*==============================================================*/
/* Table: T_OA_GIFTPLAN                                         */
/*==============================================================*/
CREATE TABLE T_OA_GIFTPLAN
(
   GIFTPLANID           VARCHAR(50) NOT NULL COMMENT '礼品派送计划ID',
   GIFTPLANCODE         VARCHAR(50) COMMENT '礼品派送计划单号',
   TITLE                VARCHAR(50) COMMENT '标题',
   APPLYSTARTDATE       DATETIME NOT NULL COMMENT '申报开始时间',
   APPLYENDDATE         DATETIME NOT NULL COMMENT '申报结束时间',
   GIFTINFO             VARCHAR(2000) COMMENT '礼品信息',
   GIFTPRICE            NUMERIC(8,0) COMMENT '礼品价格',
   UNITMASTERCOUNT      NUMERIC(8,0) COMMENT '单位最大派送数量',
   OWNERCOMPANYID       VARCHAR(50) NOT NULL COMMENT '计划负责人公司ID',
   OWNERCOMPANYNAME     VARCHAR(50) COMMENT '计划负责人公司名称',
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL COMMENT '计划负责人部门ID',
   OWNERDEPARTMENTNAME  VARCHAR(50) COMMENT '计划负责人部门名称',
   OWNERPOSTID          VARCHAR(50) NOT NULL COMMENT '计划负责人岗位ID',
   OWNERPOSTNAME        VARCHAR(50) COMMENT '计划负责人岗位名称',
   OWNERID              VARCHAR(50) NOT NULL COMMENT '计划负责人ID',
   OWNERNAME            VARCHAR(50) COMMENT '计划负责人名称',
   CREATECOMPANYID      VARCHAR(50) NOT NULL COMMENT '创建人公司ID',
   CREATECOMPANYNAME    VARCHAR(50) COMMENT '创建人公司名称',
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL COMMENT '创建人部门ID',
   CREATEDEPARTMENTNAME VARCHAR(50) COMMENT '创建人部门名称',
   CREATEPOSTID         VARCHAR(50) NOT NULL COMMENT '创建人岗位ID',
   CREATEPOSTNAME       VARCHAR(50) COMMENT '创建人岗位名称',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人ID',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人名称',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) NOT NULL COMMENT '修改人ID',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改人名称',
   UPDATEDATE           DATETIME NOT NULL COMMENT '修改时间',
   REMARK               VARCHAR(2000) COMMENT '备注',
   EDITSTATES           VARCHAR(50) NOT NULL COMMENT '///0 删除状态 Deleted
            ///1 已生效 Actived
            ///2 未生效 UnActived
            ///3 撤消中 PendingCancelled
            ///4 已撤消 Cancelled',
   CHECKSTATES          VARCHAR(50) NOT NULL COMMENT '///0 未提交 UnSubmit,
            ///1 审核中 Approving,
            /// 2 审核通过 Approved,
            ///3 审核未通过 UnApproved
            ///4 保存         Saved',
   PRIMARY KEY (GIFTPLANID)
);

ALTER TABLE T_OA_GIFTPLAN COMMENT '礼品派送计划';

/*==============================================================*/
/* Table: T_OA_GRADED                                           */
/*==============================================================*/
CREATE TABLE T_OA_GRADED
(
   GRADEDID             VARCHAR(50) NOT NULL,
   GRADED               VARCHAR(50) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (GRADED)
);

ALTER TABLE T_OA_GRADED COMMENT 'T_OA_GRADED';

/*==============================================================*/
/* Table: T_OA_HIREAPP                                          */
/*==============================================================*/
CREATE TABLE T_OA_HIREAPP
(
   HIREAPPID            VARCHAR(50) NOT NULL,
   HOUSELISTID          VARCHAR(50) NOT NULL,
   RENTCOST             NUMERIC(8,0) NOT NULL DEFAULT 0,
   DEPOSIT              NUMERIC(8,0) NOT NULL DEFAULT 0,
   MANAGECOST           NUMERIC(8,0) NOT NULL DEFAULT 0,
   STARTDATE            DATETIME NOT NULL,
   ENDDATE              DATETIME NOT NULL COMMENT '预计到期时间，实际到期以退房时间为准',
   BACKDATE             DATETIME COMMENT '退房时修改',
   RENTTYPE             VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：合租；1：整租；',
   SETTLEMENTTYPE       VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：工资扣；1：现金；',
   ISBACK               VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:未退，1：已退',
   ISOK                 VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:未确认，1：已退认，2：取消',
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；1：审核中；2：审核通过；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (HIREAPPID)
);

ALTER TABLE T_OA_HIREAPP COMMENT 'T_OA_HIREAPP';

/*==============================================================*/
/* Table: T_OA_HIRERECORD                                       */
/*==============================================================*/
CREATE TABLE T_OA_HIRERECORD
(
   HIRERECORD           VARCHAR(50) NOT NULL,
   HIREAPPID            VARCHAR(50) NOT NULL,
   RENTER               VARCHAR(50) NOT NULL DEFAULT '0' COMMENT '0：合租；1：整租；',
   RENTCOST             NUMERIC(8,0) NOT NULL DEFAULT 0,
   MANAGECOST           NUMERIC(8,0) NOT NULL DEFAULT 0,
   WATER                NUMERIC(8,0) DEFAULT 0,
   ELECTRICITY          NUMERIC(8,0) DEFAULT 0,
   OTHERCOST            NUMERIC(8,0) DEFAULT 0,
   WATERNUM             NUMERIC(8,0) DEFAULT 0,
   ELECTRICITYNUM       NUMERIC(8,0) DEFAULT 0,
   SETTLEMENTDATE       DATETIME NOT NULL,
   SETTLEMENTTYPE       VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：工资扣；1：现金；',
   ISSETTLEMENT         VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:未结算，1：已结算',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (HIRERECORD)
);

ALTER TABLE T_OA_HIRERECORD COMMENT 'T_OA_HIRERECORD';

/*==============================================================*/
/* Table: T_OA_HOUSEINFO                                        */
/*==============================================================*/
CREATE TABLE T_OA_HOUSEINFO
(
   HOUSEID              VARCHAR(50) NOT NULL,
   UPTOWN               VARCHAR(50) NOT NULL,
   HOUSENAME            VARCHAR(50) NOT NULL,
   FLOOR                NUMERIC(8,0) NOT NULL,
   ROOMCODE             VARCHAR(50) NOT NULL,
   RENTCOST             NUMERIC(8,0) NOT NULL DEFAULT 0,
   DEPOSIT              NUMERIC(8,0) NOT NULL DEFAULT 0,
   SHAREDRENTCOST       NUMERIC(8,0) NOT NULL DEFAULT 0,
   SHAREDDEPOSIT        NUMERIC(8,0) NOT NULL DEFAULT 0,
   MANAGECOST           NUMERIC(8,0) NOT NULL DEFAULT 0,
   NUMBER               NUMERIC(8,0) NOT NULL DEFAULT 0,
   RENTNUMBER           NUMERIC(8,0) NOT NULL DEFAULT 0,
   REMARK               VARCHAR(2000),
   CONTENT              LONGBLOB NOT NULL,
   ISRENT               VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:未出租，1：已出租',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL ,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (HOUSEID)
);

ALTER TABLE T_OA_HOUSEINFO COMMENT 'T_OA_HOUSEINFO';

/*==============================================================*/
/* Table: T_OA_HOUSEINFOISSUANCE                                */
/*==============================================================*/
CREATE TABLE T_OA_HOUSEINFOISSUANCE
(
   ISSUANCEID           VARCHAR(50) NOT NULL,
   ISSUANCETITLE        VARCHAR(200) NOT NULL,
   CONTENT              LONGBLOB NOT NULL,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (ISSUANCEID)
);

ALTER TABLE T_OA_HOUSEINFOISSUANCE COMMENT 'T_OA_HOUSEINFOISSUANCE';

/*==============================================================*/
/* Table: T_OA_HOUSELIST                                        */
/*==============================================================*/
CREATE TABLE T_OA_HOUSELIST
(
   HOUSELISTID          VARCHAR(50) NOT NULL,
   ISSUANCEID           VARCHAR(50) NOT NULL,
   HOUSEID              VARCHAR(50) NOT NULL,
   RENTCOST             NUMERIC(8,0) NOT NULL DEFAULT 0,
   DEPOSIT              NUMERIC(8,0) NOT NULL DEFAULT 0,
   SHAREDRENTCOST       NUMERIC(8,0) NOT NULL DEFAULT 0,
   SHAREDDEPOSIT        NUMERIC(8,0) NOT NULL DEFAULT 0,
   MANAGECOST           NUMERIC(8,0) NOT NULL DEFAULT 0,
   CONTENT              TEXT NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (HOUSELISTID)
);

ALTER TABLE T_OA_HOUSELIST COMMENT 'T_OA_HOUSELIST';

/*==============================================================*/
/* Table: T_OA_LENDARCHIVES                                     */
/*==============================================================*/
CREATE TABLE T_OA_LENDARCHIVES
(
   LENDARCHIVESID       VARCHAR(50) NOT NULL,
   ARCHIVESID           VARCHAR(50) NOT NULL,
   STARTDATE            DATETIME NOT NULL,
   ENDDATE              DATETIME,
   PLANENDDATE          DATETIME NOT NULL,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (LENDARCHIVESID)
);

ALTER TABLE T_OA_LENDARCHIVES COMMENT '只有有原件的才能借阅（OA_ARCHIVES_T中的FLAG为1）';

/*==============================================================*/
/* Table: T_OA_LICENSEDETAIL                                    */
/*==============================================================*/
CREATE TABLE T_OA_LICENSEDETAIL
(
   LICENSEDETAILID      VARCHAR(50) NOT NULL,
   LICENSEMASTERID      VARCHAR(50) NOT NULL,
   REGISTERTYPE         VARCHAR(50) NOT NULL COMMENT '记录年检还是变更等，在字典中定义',
   LEGALPERSON          VARCHAR(50) NOT NULL,
   ADDRESS              VARCHAR(200) NOT NULL,
   LICENCENO            VARCHAR(100) NOT NULL,
   BUSSINESSAREA        VARCHAR(1000) NOT NULL,
   REGISTERCHARGE       VARCHAR(50) DEFAULT '0',
   FROMDATE             DATETIME NOT NULL,
   TODATE               DATETIME,
   REMARK               VARCHAR(2000),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (LICENSEDETAILID)
);

ALTER TABLE T_OA_LICENSEDETAIL COMMENT 'T_OA_LICENSEDETAIL';

/*==============================================================*/
/* Table: T_OA_LICENSEMASTER                                    */
/*==============================================================*/
CREATE TABLE T_OA_LICENSEMASTER
(
   LICENSEMASTERID      VARCHAR(50) NOT NULL,
   ORGCODE              VARCHAR(50),
   LICENSENAME          VARCHAR(200) NOT NULL COMMENT '字典中定义',
   POSITION             VARCHAR(200) NOT NULL,
   LEGALPERSON          VARCHAR(50) NOT NULL COMMENT '从子表中读入',
   ADDRESS              VARCHAR(200) NOT NULL COMMENT '从子表中读入',
   LICENCENO            VARCHAR(100) NOT NULL COMMENT '从子表中读入',
   BUSSINESSAREA        VARCHAR(1000) NOT NULL COMMENT '从子表中读入',
   FROMDATE             DATETIME COMMENT '从子表中读入',
   TODATE               DATETIME COMMENT '从子表中读入',
   DAY                  NUMERIC(8,0) NOT NULL DEFAULT 0 COMMENT '设置到期多少天前提醒',
   LENDFLAG             VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:未借出，1：已借出',
   ISVALID              VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:无效，1：有效',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (LICENSEMASTERID)
);

ALTER TABLE T_OA_LICENSEMASTER COMMENT 'T_OA_LICENSEMASTER';

/*==============================================================*/
/* Table: T_OA_LICENSEUSER                                      */
/*==============================================================*/
CREATE TABLE T_OA_LICENSEUSER
(
   LICENSEUSERID        VARCHAR(50) NOT NULL,
   LICENSEMASTERID      VARCHAR(50) NOT NULL,
   CONTENT              VARCHAR(500) NOT NULL,
   STARTDATE            DATETIME NOT NULL,
   ENDDATE              DATETIME NOT NULL,
   HASRETURN            VARCHAR(1) NOT NULL COMMENT '0:未还；1：已还',
   CHECKSTATE           VARCHAR(1) NOT NULL COMMENT '0：未提交；1：审核通过；2：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (LICENSEUSERID)
);

ALTER TABLE T_OA_LICENSEUSER COMMENT 'T_OA_LICENSEUSER';

/*==============================================================*/
/* Table: T_OA_MAINTENANCEAPP                                   */
/*==============================================================*/
CREATE TABLE T_OA_MAINTENANCEAPP
(
   MAINTENANCEAPPID     VARCHAR(50) NOT NULL,
   ASSETID              VARCHAR(50) NOT NULL,
   MAINTENANCETYPE      VARCHAR(50) NOT NULL,
   CONTENT              VARCHAR(200) NOT NULL,
   REPAIRDATE           DATETIME NOT NULL,
   RETRIEVEDATE         DATETIME NOT NULL,
   REPAIRCOMPANY        VARCHAR(50) NOT NULL,
   TEL                  VARCHAR(50) NOT NULL,
   REMARK               VARCHAR(200),
   ISCHARGE             VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：无费用，1：有费用',
   CHARGEMONEY          NUMERIC(8,0) DEFAULT 0,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (MAINTENANCEAPPID)
);

ALTER TABLE T_OA_MAINTENANCEAPP COMMENT 'T_OA_MAINTENANCEAPP';

/*==============================================================*/
/* Table: T_OA_MAINTENANCERECORD                                */
/*==============================================================*/
CREATE TABLE T_OA_MAINTENANCERECORD
(
   MAINTENANCERECORDID  VARCHAR(50) NOT NULL,
   MAINTENANCEAPPID     VARCHAR(50) NOT NULL,
   MAINTENANCETYPE      VARCHAR(50) NOT NULL,
   CONTENT              VARCHAR(200) NOT NULL,
   REPAIRDATE           DATETIME NOT NULL,
   RETRIEVEDATE         DATETIME NOT NULL,
   REPAIRCOMPANY        VARCHAR(50) NOT NULL,
   TEL                  VARCHAR(50) NOT NULL,
   REMARK               VARCHAR(200),
   ISCHARGE             VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：无费用，1：有费用',
   CHARGEMONEY          NUMERIC(8,0) DEFAULT 0,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (MAINTENANCERECORDID)
);

ALTER TABLE T_OA_MAINTENANCERECORD COMMENT 'T_OA_MAINTENANCERECORD';

/*==============================================================*/
/* Table: T_OA_MEETINGCONTENT                                   */
/*==============================================================*/
CREATE TABLE T_OA_MEETINGCONTENT
(
   MEETINGCONTENTID     VARCHAR(50) NOT NULL,
   MEETINGINFOID        VARCHAR(50) NOT NULL,
   MEETINGUSERID        VARCHAR(50) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   CONTENT              TEXT NOT NULL,
   PRIMARY KEY (MEETINGCONTENTID)
);

ALTER TABLE T_OA_MEETINGCONTENT COMMENT 'T_OA_MEETINGCONTENT';

/*==============================================================*/
/* Table: T_OA_MEETINGINFO                                      */
/*==============================================================*/
CREATE TABLE T_OA_MEETINGINFO
(
   MEETINGINFOID        VARCHAR(50) NOT NULL,
   ISAUTO               VARCHAR(1) NOT NULL DEFAULT '0',
   MEETINGROOMNAME      VARCHAR(100),
   STARTTIME            DATETIME NOT NULL,
   ENDTIME              DATETIME NOT NULL,
   COUNT                NUMERIC(8,0) NOT NULL DEFAULT 2,
   MEETINGTITLE         VARCHAR(100) NOT NULL,
   CONTENT              LONGBLOB NOT NULL,
   DEPARTNAME           VARCHAR(100) NOT NULL,
   HOSTID               VARCHAR(50) NOT NULL,
   HOSTNAME             VARCHAR(50) NOT NULL,
   RECORDUSERID         VARCHAR(50) NOT NULL,
   RECORDUSERNAME       VARCHAR(50) NOT NULL,
   MEETINGTYPE          VARCHAR(100) NOT NULL,
   TEL                  VARCHAR(50) NOT NULL,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   ISCANCEL             VARCHAR(1) NOT NULL DEFAULT '1' COMMENT '0：取消，1：正常',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (MEETINGINFOID)
);

ALTER TABLE T_OA_MEETINGINFO COMMENT 'T_OA_MEETINGINFO';

/*==============================================================*/
/* Table: T_OA_MEETINGMESSAGE                                   */
/*==============================================================*/
CREATE TABLE T_OA_MEETINGMESSAGE
(
   MEETINGMESSAGEID     VARCHAR(50) NOT NULL DEFAULT '0',
   MEETINGINFOID        VARCHAR(50) NOT NULL,
   TITLE                VARCHAR(200) NOT NULL,
   CONTENT              LONGBLOB NOT NULL,
   TEL                  VARCHAR(50) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (MEETINGMESSAGEID)
);

ALTER TABLE T_OA_MEETINGMESSAGE COMMENT 'T_OA_MEETINGMESSAGE';

/*==============================================================*/
/* Table: T_OA_MEETINGROOM                                      */
/*==============================================================*/
CREATE TABLE T_OA_MEETINGROOM
(
   MEETINGROOMID        VARCHAR(50) NOT NULL,
   MEETINGROOMNAME      VARCHAR(100) NOT NULL,
   LOCATION             VARCHAR(100),
   COMPANYID            VARCHAR(50) NOT NULL,
   SEAT                 NUMERIC(8,0) NOT NULL,
   AREA                 NUMERIC(8,0) NOT NULL,
   ROSTRUM              VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：无，1：有',
   VIDEO                VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：无，1：有',
   AUDIO                VARCHAR(1) NOT NULL DEFAULT '1' COMMENT '0：无，1：有',
   NETWORK              VARCHAR(1) NOT NULL DEFAULT '1' COMMENT '0：无，1：有',
   WIFI                 VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：无，1：有',
   TEL                  VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：无，1：有',
   PROJECTOR            VARCHAR(1) NOT NULL DEFAULT '1' COMMENT '0：无，1：有',
   AIRCONDITIONING      VARCHAR(1) NOT NULL DEFAULT '1' COMMENT '0：无，1：有',
   WATERDISPENSER       VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：无，1：有',
   REMARK               VARCHAR(200),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (MEETINGROOMNAME)
);

ALTER TABLE T_OA_MEETINGROOM COMMENT 'T_OA_MEETINGROOM';

/*==============================================================*/
/* Table: T_OA_MEETINGROOMAPP                                   */
/*==============================================================*/
CREATE TABLE T_OA_MEETINGROOMAPP
(
   MEETINGROOMAPPID     VARCHAR(50) NOT NULL,
   MEETINGROOMNAME      VARCHAR(100) NOT NULL,
   STARTTIME            DATETIME NOT NULL,
   ENDTIME              DATETIME NOT NULL,
   DEPARTNAME           VARCHAR(100) NOT NULL,
   TEL                  VARCHAR(50) NOT NULL,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；1：审批中；2：审批通过，3：审批不通过',
   ISCANCEL             VARCHAR(1) NOT NULL DEFAULT '1' COMMENT '0：取消，1：正常',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (MEETINGROOMAPPID)
);

ALTER TABLE T_OA_MEETINGROOMAPP COMMENT 'T_OA_MEETINGROOMAPP';

/*==============================================================*/
/* Table: T_OA_MEETINGROOMTIMECHANGE                            */
/*==============================================================*/
CREATE TABLE T_OA_MEETINGROOMTIMECHANGE
(
   MEETINGROOMTIMECHANGEID VARCHAR(50) NOT NULL,
   MEETINGROOMAPPID     VARCHAR(50) NOT NULL,
   STARTTIME            DATETIME NOT NULL,
   ENDTIME              DATETIME NOT NULL,
   REASON               VARCHAR(2000),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (MEETINGROOMTIMECHANGEID)
);

ALTER TABLE T_OA_MEETINGROOMTIMECHANGE COMMENT 'T_OA_MEETINGROOMTIMECHANGE';

/*==============================================================*/
/* Table: T_OA_MEETINGSTAFF                                     */
/*==============================================================*/
CREATE TABLE T_OA_MEETINGSTAFF
(
   MEETINGSTAFFID       VARCHAR(50),
   MEETINGINFOID        VARCHAR(50) NOT NULL,
   MEETINGUSERID        VARCHAR(50) NOT NULL,
   FILENAME             VARCHAR(50),
   CONFIRMFLAG          VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:未确认；1：已确认,2:不参加,但上传材料,3:不参加',
   REMARK               VARCHAR(200),
   ISOK                 VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:未确认；1：已确认',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL ,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (MEETINGINFOID, MEETINGUSERID)
);

ALTER TABLE T_OA_MEETINGSTAFF COMMENT 'T_OA_MEETINGSTAFF';

/*==============================================================*/
/* Table: T_OA_MEETINGTEMPLATE                                  */
/*==============================================================*/
CREATE TABLE T_OA_MEETINGTEMPLATE
(
   MEETINGTEMPLATEID    VARCHAR(50) NOT NULL,
   TEMPLATENAME         VARCHAR(100) NOT NULL,
   MEETINGTYPE          VARCHAR(100) NOT NULL,
   CONTENT              LONGBLOB NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (MEETINGTEMPLATEID)
);

ALTER TABLE T_OA_MEETINGTEMPLATE COMMENT 'T_OA_MEETINGTEMPLATE';

/*==============================================================*/
/* Table: T_OA_MEETINGTIMECHANGE                                */
/*==============================================================*/
CREATE TABLE T_OA_MEETINGTIMECHANGE
(
   MEETINGTIMECHANGEID  VARCHAR(50) NOT NULL,
   MEETINGINFOID        VARCHAR(50) NOT NULL,
   STARTTIME            DATETIME NOT NULL,
   ENDTIME              DATETIME NOT NULL,
   REASON               VARCHAR(2000),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (MEETINGTIMECHANGEID)
);

ALTER TABLE T_OA_MEETINGTIMECHANGE COMMENT 'T_OA_MEETINGTIMECHANGE';

/*==============================================================*/
/* Table: T_OA_MEETINGTYPE                                      */
/*==============================================================*/
CREATE TABLE T_OA_MEETINGTYPE
(
   MEETINGTYPEID        VARCHAR(50) NOT NULL,
   MEETINGTYPE          VARCHAR(100) NOT NULL,
   ISAUTO               VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：否，1：是',
   CYCLE                NUMERIC(8,0),
   REMINDDAY            NUMERIC(8,0),
   CONVENER             VARCHAR(50),
   CONVENERNAME         VARCHAR(50),
   CONTENT              LONGBLOB,
   REMARK               VARCHAR(1000),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (MEETINGTYPE)
);

ALTER TABLE T_OA_MEETINGTYPE COMMENT 'T_OA_MEETINGTYPE';

/*==============================================================*/
/* Table: T_OA_ORDERMEAL                                        */
/*==============================================================*/
CREATE TABLE T_OA_ORDERMEAL
(
   ORDERMEALID          VARCHAR(50) NOT NULL,
   ORDERMEALTITLE       VARCHAR(100) NOT NULL,
   CONTENT              VARCHAR(1000) NOT NULL DEFAULT '0' COMMENT '0:未订餐;1:已订餐',
   REMARK               VARCHAR(2000),
   ORDERMEALFLAG        VARCHAR(1) NOT NULL COMMENT '0:取消，1：已订,2:待订',
   TEL                  VARCHAR(50) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (ORDERMEALID)
);

ALTER TABLE T_OA_ORDERMEAL COMMENT 'T_OA_ORDERMEAL';

/*==============================================================*/
/* Table: T_OA_ORGANIZATION                                     */
/*==============================================================*/
CREATE TABLE T_OA_ORGANIZATION
(
   ORGANIZATIONID       VARCHAR(50) NOT NULL,
   ORGCODE              VARCHAR(50) NOT NULL,
   ORGNAME              VARCHAR(200) NOT NULL,
   LEGALPERSON          VARCHAR(50) NOT NULL,
   ADDRESS              VARCHAR(200) NOT NULL,
   LICENCENO            VARCHAR(100) NOT NULL,
   BUSSINESSAREA        VARCHAR(1000) NOT NULL,
   ISCHARGE             VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：无费用，1：有费用',
   CHARGEMONEY          NUMERIC(8,0) DEFAULT 0,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (ORGCODE)
);

ALTER TABLE T_OA_ORGANIZATION COMMENT 'T_OA_ORGANIZATION';

/*==============================================================*/
/* Table: T_OA_PRIORITIES                                       */
/*==============================================================*/
CREATE TABLE T_OA_PRIORITIES
(
   PRIORITIESID         VARCHAR(50) NOT NULL,
   PRIORITIES           VARCHAR(50) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (PRIORITIES)
);

ALTER TABLE T_OA_PRIORITIES COMMENT 'T_OA_PRIORITIES';

/*==============================================================*/
/* Table: T_OA_PROGRAMAPPLICATIONS                              */
/*==============================================================*/
CREATE TABLE T_OA_PROGRAMAPPLICATIONS
(
   PROGRAMAPPLICATIONSID VARCHAR(50) NOT NULL COMMENT '方案应用ID',
   TRAVELSOLUTIONSID    VARCHAR(50) COMMENT '方案id',
   COMPANYID            VARCHAR(50) COMMENT '公司ID',
   OWNERCOMPANYID       VARCHAR(50) NOT NULL COMMENT '所属公司',
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL COMMENT '所属部门',
   OWNERPOSTID          VARCHAR(50) NOT NULL COMMENT '所属岗位',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人',
   CREATEDATE           DATETIME NOT NULL  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   OWNERID              VARCHAR(50) COMMENT '所属人ID',
   PRIMARY KEY (PROGRAMAPPLICATIONSID)
);

ALTER TABLE T_OA_PROGRAMAPPLICATIONS COMMENT '出差方案应用';

/*==============================================================*/
/* Table: T_OA_REIMBURSEMENTDETAIL                              */
/*==============================================================*/
CREATE TABLE T_OA_REIMBURSEMENTDETAIL
(
   REIMBURSEMENTDETAILID VARCHAR(50) NOT NULL COMMENT 'REIMBURSEMENTDETAILID',
   TRAVELREIMBURSEMENTID VARCHAR(50) COMMENT '出差报销ID',
   STARTDATE            DATETIME COMMENT '开始时间',
   ENDDATE              DATETIME COMMENT '结束时间',
   DEPCITY              VARCHAR(50) NOT NULL COMMENT '通过SYS_TYPEGROUP_T获取地区定义名',
   DESTCITY             VARCHAR(50) NOT NULL COMMENT '通过SYS_TYPEGROUP_T获取地区定义名',
   BUSINESSDAYS         VARCHAR(50) COMMENT '总计出差的时间',
   TYPEOFTRAVELTOOLS    VARCHAR(50) COMMENT '乘坐的交通工具类型',
   TAKETHETOOLLEVEL     VARCHAR(50) COMMENT '交通工具的级别(例如：飞机头等舱)',
   TRANSPORTCOSTS       NUMERIC(8,0) COMMENT '乘坐交通工具费用',
   ACCOMMODATION        NUMERIC(8,0) COMMENT '住宿补贴费用',
   TRANSPORTATIONSUBSIDIES NUMERIC(8,0) COMMENT '交通补贴费用',
   MEALSUBSIDIES        NUMERIC(8,0) COMMENT '餐费补贴',
   OTHERCOSTS           NUMERIC(8,0) COMMENT '其他费用',
   PRIVATEAFFAIR        VARCHAR(1) COMMENT '私事',
   CREATEUSERNAME       VARCHAR(50) NOT NULL COMMENT '创建人名',
   CREATEDATE           DATETIME NOT NULL  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   GOOUTTOMEET          VARCHAR(1),
   COMPANYCAR           VARCHAR(1) COMMENT '公司派车',
   THENUMBEROFNIGHTS    VARCHAR(50) COMMENT '住宿天数',
   STARTCITYNAME        VARCHAR(2000),
   ENDCITYNAME          VARCHAR(2000),
   COMPUTINGTIME        VARCHAR(50) COMMENT '总计出差的时间',
   PRIMARY KEY (REIMBURSEMENTDETAILID)
);

ALTER TABLE T_OA_REIMBURSEMENTDETAIL COMMENT '出差报销子表';

/*==============================================================*/
/* Table: T_OA_REQUIRE                                          */
/*==============================================================*/
CREATE TABLE T_OA_REQUIRE
(
   REQUIREID            VARCHAR(50) NOT NULL,
   REQUIREMASTERID      VARCHAR(50) NOT NULL,
   APPTITLE             VARCHAR(100) NOT NULL,
   CONTENT              VARCHAR(2000) NOT NULL,
   STARTDATE            DATETIME NOT NULL,
   ENDDATE              DATETIME NOT NULL,
   OPTFLAG              VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：可不填，1：必填',
   WAY                  VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：匿名，1：实名',
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (REQUIREID)
);

ALTER TABLE T_OA_REQUIRE COMMENT 'T_OA_REQUIRE';

/*==============================================================*/
/* Table: T_OA_REQUIREDETAIL                                    */
/*==============================================================*/
CREATE TABLE T_OA_REQUIREDETAIL
(
   REQUIREDETAILID      VARCHAR(50) NOT NULL,
   REQUIREMASTERID      VARCHAR(50),
   SUBJECTID            NUMERIC(8,0) NOT NULL COMMENT '题目顺序号',
   CODE                 VARCHAR(100) NOT NULL COMMENT '答案显示代码，如1，2，3，A，B，C',
   CONTENT              VARCHAR(200) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (REQUIREDETAILID)
);

ALTER TABLE T_OA_REQUIREDETAIL COMMENT 'T_OA_REQUIREDETAIL';

/*==============================================================*/
/* Table: T_OA_REQUIREDETAIL2                                   */
/*==============================================================*/
CREATE TABLE T_OA_REQUIREDETAIL2
(
   REQUIREDETAIL2ID     VARCHAR(50) NOT NULL,
   REQUIREMASTERID      VARCHAR(50) NOT NULL,
   SUBJECTID            NUMERIC(8,0) NOT NULL,
   CONTENT              VARCHAR(200) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (REQUIREMASTERID, SUBJECTID)
);

ALTER TABLE T_OA_REQUIREDETAIL2 COMMENT 'T_OA_REQUIREDETAIL2';

/*==============================================================*/
/* Table: T_OA_REQUIREDISTRIBUTE                                */
/*==============================================================*/
CREATE TABLE T_OA_REQUIREDISTRIBUTE
(
   REQUIREDISTRIBUTEID  VARCHAR(50) NOT NULL,
   REQUIREID            VARCHAR(50) NOT NULL,
   DISTRIBUTETITLE      VARCHAR(100) NOT NULL,
   CONTENT              VARCHAR(2000) NOT NULL,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (REQUIREDISTRIBUTEID)
);

ALTER TABLE T_OA_REQUIREDISTRIBUTE COMMENT 'T_OA_REQUIREDISTRIBUTE';

/*==============================================================*/
/* Table: T_OA_REQUIREMASTER                                    */
/*==============================================================*/
CREATE TABLE T_OA_REQUIREMASTER
(
   REQUIREMASTERID      VARCHAR(50) NOT NULL,
   REQUIRETITLE         VARCHAR(100) NOT NULL,
   CONTENT              TEXT NOT NULL,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (REQUIREMASTERID)
);

ALTER TABLE T_OA_REQUIREMASTER COMMENT 'T_OA_REQUIREMASTER';

/*==============================================================*/
/* Table: T_OA_REQUIRERESULT                                    */
/*==============================================================*/
CREATE TABLE T_OA_REQUIRERESULT
(
   REQUIRERESULTID      VARCHAR(50) NOT NULL,
   REQUIREID            VARCHAR(50) NOT NULL,
   REQUIREMASTERID      VARCHAR(50) NOT NULL,
   SUBJECTID            NUMERIC(8,0) NOT NULL,
   RESULT               VARCHAR(200) NOT NULL,
   CONTENT              VARCHAR(500),
   UPDATEDATE           DATETIME,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   CODE                 VARCHAR(50),
   PRIMARY KEY (REQUIRERESULTID)
);

ALTER TABLE T_OA_REQUIRERESULT COMMENT 'T_OA_REQUIRERESULT';

/*==============================================================*/
/* Table: T_OA_SATISFACTIONDETAIL                               */
/*==============================================================*/
CREATE TABLE T_OA_SATISFACTIONDETAIL
(
   SATISFACTIONDETAILID VARCHAR(50) NOT NULL,
   SATISFACTIONMASTERID VARCHAR(50) NOT NULL,
   SUBJECTID            NUMERIC(8,0) NOT NULL,
   CONTENT              VARCHAR(200) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (SATISFACTIONMASTERID, SUBJECTID)
);

ALTER TABLE T_OA_SATISFACTIONDETAIL COMMENT 'T_OA_SATISFACTIONDETAIL';

/*==============================================================*/
/* Table: T_OA_SATISFACTIONDISTRIBUTE                           */
/*==============================================================*/
CREATE TABLE T_OA_SATISFACTIONDISTRIBUTE
(
   SATISFACTIONDISTRIBUTEID VARCHAR(50) NOT NULL,
   SATISFACTIONREQUIREID VARCHAR(50) NOT NULL,
   DISTRIBUTETITLE      VARCHAR(100) NOT NULL,
   CONTENT              VARCHAR(2000) NOT NULL,
   ANSWERID             VARCHAR(50) NOT NULL COMMENT '设置选中答案ID以上的为满意度满意结果',
   PERCENTAGE           NUMERIC(8,0) NOT NULL DEFAULT 0 COMMENT '设置百分比多少为满意',
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (SATISFACTIONDISTRIBUTEID)
);

ALTER TABLE T_OA_SATISFACTIONDISTRIBUTE COMMENT 'T_OA_SATISFACTIONDISTRIBUTE';

/*==============================================================*/
/* Table: T_OA_SATISFACTIONMASTER                               */
/*==============================================================*/
CREATE TABLE T_OA_SATISFACTIONMASTER
(
   SATISFACTIONMASTERID VARCHAR(50) NOT NULL,
   SATISFACTIONTITLE    VARCHAR(100) NOT NULL,
   CONTENT              TEXT NOT NULL,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (SATISFACTIONMASTERID)
);

ALTER TABLE T_OA_SATISFACTIONMASTER COMMENT 'T_OA_SATISFACTIONMASTER';

/*==============================================================*/
/* Table: T_OA_SATISFACTIONREQUIRE                              */
/*==============================================================*/
CREATE TABLE T_OA_SATISFACTIONREQUIRE
(
   SATISFACTIONREQUIREID VARCHAR(50) NOT NULL,
   SATISFACTIONMASTERID VARCHAR(50) NOT NULL,
   SATISFACTIONTITLE    VARCHAR(100) NOT NULL,
   CONTENT              VARCHAR(2000) NOT NULL,
   ANSWERGROUPID        VARCHAR(50) NOT NULL,
   STARTDATE            DATETIME NOT NULL,
   ENDDATE              DATETIME NOT NULL,
   OPTFLAG              VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：可不填，1：必填',
   WAY                  VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：匿名，1：实名',
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (SATISFACTIONREQUIREID)
);

ALTER TABLE T_OA_SATISFACTIONREQUIRE COMMENT 'T_OA_SATISFACTIONREQUIRE';

/*==============================================================*/
/* Table: T_OA_SATISFACTIONRESULT                               */
/*==============================================================*/
CREATE TABLE T_OA_SATISFACTIONRESULT
(
   SATISFACTIONRESULTID VARCHAR(50) NOT NULL,
   SATISFACTIONREQUIREID VARCHAR(50) NOT NULL,
   SUBJECTID            NUMERIC(8,0) NOT NULL,
   RESULT               VARCHAR(200) NOT NULL,
   CONTENT              VARCHAR(500),
   UPDATEDATE           DATETIME,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   PRIMARY KEY (SATISFACTIONRESULTID)
);

ALTER TABLE T_OA_SATISFACTIONRESULT COMMENT 'T_OA_SATISFACTIONRESULT';

/*==============================================================*/
/* Table: T_OA_SENDDOC                                          */
/*==============================================================*/
CREATE TABLE T_OA_SENDDOC
(
   SENDDOCID            VARCHAR(50) NOT NULL,
   GRADED               VARCHAR(50),
   PRIORITIES           VARCHAR(50),
   SENDDOCTITLE         VARCHAR(100),
   KEYWORDS             VARCHAR(50),
   SEND                 VARCHAR(200),
   CC                   VARCHAR(200),
   SENDDOCTYPE          VARCHAR(50),
   CONTENT              LONGBLOB,
   DEPARTID             VARCHAR(50),
   NUM                  VARCHAR(100),
   TEL                  VARCHAR(50),
   CHECKSTATE           VARCHAR(1) DEFAULT '0' COMMENT '0:未提交;1:审批中，2：审批通过，3：未通过',
   ISDISTRIBUTE         VARCHAR(1) COMMENT '0:未分发，1：已分发',
   ISSAVE               VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:未归档，1：已归档',
   OWNERID              VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEUSERNAME       VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDATE           DATETIME,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PUBLISHDATE          DATETIME COMMENT '发布时间',
   ISREDDOC             VARCHAR(1) DEFAULT '0',
   ISTOP                VARCHAR(1) DEFAULT '0' COMMENT '是否置顶：0不置顶 1置顶',
   TOPSTARTDATE         DATETIME COMMENT '置顶开始日期',
   TOPENDDATE           DATETIME COMMENT '置顶结束日期',
   PRIMARY KEY (SENDDOCID)
);

ALTER TABLE T_OA_SENDDOC COMMENT 'T_OA_SENDDOC';

/*==============================================================*/
/* Table: T_OA_SENDDOCTEMPLATE                                  */
/*==============================================================*/
CREATE TABLE T_OA_SENDDOCTEMPLATE
(
   SENDDOCTEMPLATEID    VARCHAR(50) NOT NULL,
   TEMPLATENAME         VARCHAR(50) NOT NULL,
   GRADED               VARCHAR(50) NOT NULL,
   PRIORITIES           VARCHAR(50) NOT NULL,
   SENDDOCTITLE         VARCHAR(100) NOT NULL,
   SENDDOCTYPE          VARCHAR(50) NOT NULL,
   CONTENT              LONGBLOB NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (SENDDOCTEMPLATEID)
);

ALTER TABLE T_OA_SENDDOCTEMPLATE COMMENT 'T_OA_SENDDOCTEMPLATE';

/*==============================================================*/
/* Table: T_OA_SENDDOCTYPE                                      */
/*==============================================================*/
CREATE TABLE T_OA_SENDDOCTYPE
(
   SENDDOCTYPEID        VARCHAR(50) NOT NULL,
   SENDDOCTYPE          VARCHAR(50) NOT NULL,
   OPTFLAG              VARCHAR(1) NOT NULL COMMENT '0:不归档，1:归档',
   REMARK               VARCHAR(200) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (SENDDOCTYPE)
);

ALTER TABLE T_OA_SENDDOCTYPE COMMENT 'T_OA_SENDDOCTYPE';

/*==============================================================*/
/* Table: T_OA_TAKETHESTANDARDTRANSPORT                         */
/*==============================================================*/
CREATE TABLE T_OA_TAKETHESTANDARDTRANSPORT
(
   TAKETHESTANDARDTRANSPORTID VARCHAR(50) NOT NULL COMMENT 'ID',
   TRAVELSOLUTIONSID    VARCHAR(50) COMMENT '方案id',
   STARTPOSTLEVEL       VARCHAR(50) COMMENT '岗位的等级，如（H级）',
   ENDPOSTLEVEL         VARCHAR(100) COMMENT '岗位的等级，如（H级）',
   LANDTIME             NUMERIC(8,0) COMMENT '陆路所需时间，从飞机线路设置里面带出来',
   TYPEOFTRAVELTOOLS    VARCHAR(50) COMMENT '工具的类型(如：飞机)',
   TAKETHETOOLLEVEL     VARCHAR(50) COMMENT '例如：飞机（头等舱）',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人',
   CREATEDATE           DATETIME NOT NULL  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   PRIMARY KEY (TAKETHESTANDARDTRANSPORTID)
);

ALTER TABLE T_OA_TAKETHESTANDARDTRANSPORT COMMENT '出差交通工具乘坐标准设置';

/*==============================================================*/
/* Table: T_OA_TRAVELREIMBURSEMENT                              */
/*==============================================================*/
CREATE TABLE T_OA_TRAVELREIMBURSEMENT
(
   TRAVELREIMBURSEMENTID VARCHAR(50) NOT NULL COMMENT '出差报销ID',
   BUSINESSREPORTID     VARCHAR(50) COMMENT '出差报告ID',
   CLAIMSWERE           VARCHAR(50) COMMENT '报销人',
   CLAIMSWERENAME       VARCHAR(50) COMMENT '报销姓名',
   REIMBURSEMENTTIME    DATETIME  COMMENT '报销时间',
   COMPUTINGTIME        VARCHAR(50) COMMENT '总计出差的时间',
   TEL                  VARCHAR(50) COMMENT '联系电话',
   THETOTALCOST         NUMERIC(8,0) COMMENT '本次出差的费用总和',
   REIMBURSEMENTOFCOSTS NUMERIC(8,0) COMMENT '本次出差的报销费用总和',
   CHECKSTATE           VARCHAR(1) DEFAULT '0' COMMENT '0：未提交；1：审核中；2：审核通过；3：审核未通过',
   REMARKS              VARCHAR(1000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属人ID',
   OWNERNAME            VARCHAR(50) COMMENT '所属人名称',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人名',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建部门ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建岗位ID',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   NOBUDGETCLAIMS       VARCHAR(50),
   BUSINESSTRIPID       VARCHAR(50) COMMENT '出差申请ID',
   CONTENT              VARCHAR(2000) COMMENT '报告内容',
   OWNERPOSTNAME        VARCHAR(2000),
   OWNERDEPARTMENTNAME  VARCHAR(2000),
   OWNERCOMPANYNAME     VARCHAR(2000),
   POSTLEVEL            VARCHAR(2000),
   STARTCITYNAME        VARCHAR(2000),
   ENDCITYNAME          VARCHAR(2000),
   ISFROMWP             VARCHAR(2000) COMMENT '来自工作计划',
   PRIMARY KEY (TRAVELREIMBURSEMENTID)
);

ALTER TABLE T_OA_TRAVELREIMBURSEMENT COMMENT '出差报销';

/*==============================================================*/
/* Table: T_OA_TRAVELSOLUTIONS                                  */
/*==============================================================*/
CREATE TABLE T_OA_TRAVELSOLUTIONS
(
   TRAVELSOLUTIONSID    VARCHAR(50) NOT NULL COMMENT '方案id',
   PROGRAMMENAME        VARCHAR(50) COMMENT '当天往返是否算一天',
   ANDFROMTHATDAY       VARCHAR(50) COMMENT '当天往返是否算一天',
   CUSTOMDAY            VARCHAR(20) COMMENT '多少小时算一天',
   CUSTOMHALFDAY        VARCHAR(20) COMMENT '多少小时算半天',
   RANGEPOSTLEVEL       VARCHAR(20) NOT NULL COMMENT '大于一定级别的不能申请费用报销',
   RANGEDAYS            VARCHAR(20) NOT NULL COMMENT '多少范围的天数内能够报销',
   MAXIMUMRANGEDAYS     VARCHAR(20) NOT NULL COMMENT '区间最大天数',
   MINIMUMINTERVALDAYS  VARCHAR(20) NOT NULL COMMENT '区间最小天数',
   INTERVALRATIO        VARCHAR(20) NOT NULL COMMENT '区间比例',
   OWNERCOMPANYID       VARCHAR(50) NOT NULL COMMENT '所属公司',
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL COMMENT '所属部门',
   OWNERPOSTID          VARCHAR(50) NOT NULL COMMENT '所属岗位',
   CREATEUSERID         VARCHAR(50) NOT NULL COMMENT '创建人',
   CREATEUSERNAME       VARCHAR(50) NOT NULL COMMENT '创建人名',
   CREATECOMPANYID      VARCHAR(50) NOT NULL COMMENT '创建公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL COMMENT '创建部门ID',
   CREATEPOSTID         VARCHAR(50) NOT NULL COMMENT '创建岗位ID',
   CREATEDATE           DATETIME NOT NULL  COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改人名',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   NOALLOWANCEPOSTLEVEL VARCHAR(50) COMMENT '无补贴的最小岗位级别',
   PRIMARY KEY (TRAVELSOLUTIONSID)
);

ALTER TABLE T_OA_TRAVELSOLUTIONS COMMENT '出差方案';

/*==============================================================*/
/* Table: T_OA_VEHICLE                                          */
/*==============================================================*/
CREATE TABLE T_OA_VEHICLE
(
   VEHICLEID            VARCHAR(50) NOT NULL,
   ASSETID              VARCHAR(50) NOT NULL,
   VEHICLEMODEL         VARCHAR(50) NOT NULL,
   VIN                  VARCHAR(50) NOT NULL,
   VEHICLEBRANDS        VARCHAR(50) NOT NULL,
   VEHICLETYPE          VARCHAR(50) NOT NULL,
   WEIGHT               NUMERIC(8,0),
   SEATQUANTITY         NUMERIC(8,0),
   BUYDATE              DATETIME NOT NULL,
   BUYPRICE             NUMERIC(8,0) NOT NULL,
   INITIALRANGE         NUMERIC(8,0) NOT NULL,
   INTERVALRANGE        NUMERIC(8,0) NOT NULL,
   MAINTENANCECYCLE     NUMERIC(8,0) NOT NULL,
   MAINTENANCEREMIND    NUMERIC(8,0) NOT NULL,
   MAINTAINCOMPANY      VARCHAR(50) NOT NULL,
   MAINTAINTEL          VARCHAR(50) NOT NULL,
   COMPANYID            VARCHAR(50) NOT NULL,
   VEHICLEFLAG          VARCHAR(1) DEFAULT '0' COMMENT '0:未使用，1：已使用',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (ASSETID)
);

ALTER TABLE T_OA_VEHICLE COMMENT 'T_OA_VEHICLE';

/*==============================================================*/
/* Table: T_OA_VEHICLECARD                                      */
/*==============================================================*/
CREATE TABLE T_OA_VEHICLECARD
(
   VEHICLECARDID        VARCHAR(50) NOT NULL,
   ASSETID              VARCHAR(50) NOT NULL,
   CARDNAME             VARCHAR(50) NOT NULL,
   CARDTYPE             VARCHAR(50) NOT NULL,
   EFFECTDATE           DATETIME NOT NULL,
   INVALIDDATE          DATETIME NOT NULL,
   CHARGEMONEY          NUMERIC(8,0) DEFAULT 0,
   CONTENT              VARCHAR(200) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (VEHICLECARDID)
);

ALTER TABLE T_OA_VEHICLECARD COMMENT 'T_OA_VEHICLECARD';

/*==============================================================*/
/* Table: T_OA_VEHICLEDISPATCH                                  */
/*==============================================================*/
CREATE TABLE T_OA_VEHICLEDISPATCH
(
   VEHICLEDISPATCHID    VARCHAR(50) NOT NULL,
   ASSETID              VARCHAR(50),
   STARTTIME            DATETIME NOT NULL,
   ENDTIME              DATETIME NOT NULL,
   NUM                  VARCHAR(50) NOT NULL DEFAULT '1',
   ROUTE                VARCHAR(200) NOT NULL,
   DRIVER               VARCHAR(50) NOT NULL,
   TEL                  VARCHAR(50) NOT NULL,
   CONTENT              VARCHAR(200),
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   ISCANCEL             VARCHAR(1) NOT NULL DEFAULT '1' COMMENT '0：取消，1：正常',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (VEHICLEDISPATCHID)
);

ALTER TABLE T_OA_VEHICLEDISPATCH COMMENT 'T_OA_VEHICLEDISPATCH';

/*==============================================================*/
/* Table: T_OA_VEHICLEDISPATCHDETAIL                            */
/*==============================================================*/
CREATE TABLE T_OA_VEHICLEDISPATCHDETAIL
(
   VEHICLEDISPATCHDETAILID VARCHAR(50) NOT NULL,
   VEHICLEDISPATCHID    VARCHAR(50) NOT NULL,
   VEHICLEUSEAPPID      VARCHAR(50) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (VEHICLEDISPATCHDETAILID)
);

ALTER TABLE T_OA_VEHICLEDISPATCHDETAIL COMMENT 'T_OA_VEHICLEDISPATCHDETAIL';

/*==============================================================*/
/* Table: T_OA_VEHICLEDISPATCHRECORD                            */
/*==============================================================*/
CREATE TABLE T_OA_VEHICLEDISPATCHRECORD
(
   VEHICLEDISPATCHRECORDID VARCHAR(50) NOT NULL,
   VEHICLEDISPATCHDETAILID VARCHAR(50) NOT NULL,
   STARTTIME            DATETIME NOT NULL,
   ENDTIME              DATETIME NOT NULL,
   NUM                  VARCHAR(50) NOT NULL DEFAULT '1',
   ROUTE                VARCHAR(200) NOT NULL,
   TEL                  VARCHAR(50) NOT NULL,
   FUEL                 NUMERIC(8,0) NOT NULL,
   THISRANGE            NUMERIC(8,0) NOT NULL,
   CONTENT              VARCHAR(200),
   ISCHARGE             VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：无费用，1：有费用',
   CHARGEMONEY          NUMERIC(8,0) DEFAULT 0,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (VEHICLEDISPATCHRECORDID)
);

ALTER TABLE T_OA_VEHICLEDISPATCHRECORD COMMENT 'T_OA_VEHICLEDISPATCHRECORD';

/*==============================================================*/
/* Table: T_OA_VEHICLEUSEAPP                                    */
/*==============================================================*/
CREATE TABLE T_OA_VEHICLEUSEAPP
(
   VEHICLEUSEAPPID      VARCHAR(50) NOT NULL,
   STARTTIME            DATETIME NOT NULL,
   ENDTIME              DATETIME NOT NULL,
   NUM                  VARCHAR(50) NOT NULL DEFAULT '1',
   ROUTE                VARCHAR(200) NOT NULL,
   TEL                  VARCHAR(50) NOT NULL,
   CONTENT              VARCHAR(200),
   DEPARTMENTID         VARCHAR(50) NOT NULL,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (VEHICLEUSEAPPID)
);

ALTER TABLE T_OA_VEHICLEUSEAPP COMMENT 'T_OA_VEHICLEUSEAPP';

/*==============================================================*/
/* Table: T_OA_VIEWSENDDOC                                      */
/*==============================================================*/
CREATE TABLE T_OA_VIEWSENDDOC
(
   VIEWSENDDOCID        VARCHAR(50) NOT NULL,
   SENDDOCID            VARCHAR(50),
   OWNERID              VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   OWNERCOMPANYNAME     VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERDEPARTMENTNAME  VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERPOSTNAME        VARCHAR(50),
   CREATEDATE           DATETIME,
   ISVIEW               VARCHAR(2) COMMENT '是否查看 0：未查看 1：已查看',
   CREATEUSERID         VARCHAR(50),
   CREATEUSERNAME       VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   PRIMARY KEY (VIEWSENDDOCID)
);

ALTER TABLE T_OA_VIEWSENDDOC COMMENT 'T_OA_VIEWSENDDOC';

/*==============================================================*/
/* Table: T_OA_WELFAREDETAIL                                    */
/*==============================================================*/
CREATE TABLE T_OA_WELFAREDETAIL
(
   WELFAREDETAILID      VARCHAR(50) NOT NULL,
   WELFAREID            VARCHAR(50) NOT NULL,
   POSTID               VARCHAR(50),
   POSTLEVELA           VARCHAR(50),
   POSTLEVELB           VARCHAR(50),
   ISLEVEL              VARCHAR(1) NOT NULL COMMENT '0：岗位，1：级别',
   STANDARD             NUMERIC(8,0) DEFAULT 0,
   REMARK               VARCHAR(1000),
   PRIMARY KEY (WELFAREDETAILID)
);

ALTER TABLE T_OA_WELFAREDETAIL COMMENT 'T_OA_WELFAREDETAIL';

/*==============================================================*/
/* Table: T_OA_WELFAREDISTRIBUTEDETAIL                          */
/*==============================================================*/
CREATE TABLE T_OA_WELFAREDISTRIBUTEDETAIL
(
   WELFAREDISTRIBUTEDETAILID VARCHAR(50) NOT NULL,
   WELFAREDISTRIBUTEMASTERID VARCHAR(50),
   USERID               VARCHAR(50) NOT NULL,
   STANDARD             NUMERIC(8,0) DEFAULT 0,
   REMARK               VARCHAR(1000),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (WELFAREDISTRIBUTEDETAILID)
);

ALTER TABLE T_OA_WELFAREDISTRIBUTEDETAIL COMMENT '按福利标准生成每个人本次发放记录';

/*==============================================================*/
/* Table: T_OA_WELFAREDISTRIBUTEMASTER                          */
/*==============================================================*/
CREATE TABLE T_OA_WELFAREDISTRIBUTEMASTER
(
   WELFAREDISTRIBUTEMASTERID VARCHAR(50) NOT NULL,
   WELFAREID            VARCHAR(50),
   WELFAREDISTRIBUTETITLE VARCHAR(1000) NOT NULL,
   DISTRIBUTEDATE       DATETIME NOT NULL,
   CONTENT              VARCHAR(2000) NOT NULL,
   ISWAGE               VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0:非随工资发，1：随工资发',
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；2：审核通过；1：审核中；3：审核未通过',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (WELFAREDISTRIBUTEMASTERID)
);

ALTER TABLE T_OA_WELFAREDISTRIBUTEMASTER COMMENT 'T_OA_WELFAREDISTRIBUTEMASTER';

/*==============================================================*/
/* Table: T_OA_WELFAREDISTRIBUTEUNDO                            */
/*==============================================================*/
CREATE TABLE T_OA_WELFAREDISTRIBUTEUNDO
(
   WELFAREDISTRIBUTEUNDOID VARCHAR(50) NOT NULL,
   WELFAREDISTRIBUTEMASTERID VARCHAR(50) NOT NULL,
   TEL                  VARCHAR(50) NOT NULL,
   CHECKSTATE           VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0：未提交；1：审核通过；2：审核中；3：审核未通过',
   REMARK               VARCHAR(2000) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (WELFAREDISTRIBUTEUNDOID)
);

ALTER TABLE T_OA_WELFAREDISTRIBUTEUNDO COMMENT 'T_OA_WELFAREDISTRIBUTEUNDO';

/*==============================================================*/
/* Table: T_OA_WELFAREMASERT                                    */
/*==============================================================*/
CREATE TABLE T_OA_WELFAREMASERT
(
   WELFAREID            VARCHAR(50) NOT NULL,
   WELFAREPROID         VARCHAR(50) NOT NULL,
   TEL                  VARCHAR(50) NOT NULL,
   STARTDATE            DATETIME NOT NULL,
   ENDDATE              DATETIME,
   REMARK               VARCHAR(1000),
   CHECKSTATE           VARCHAR(1) NOT NULL COMMENT '0：未提交；1：审核通过；2：审核中；3：审核未通过',
   COMPANYID            VARCHAR(50) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL ,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (WELFAREID)
);

ALTER TABLE T_OA_WELFAREMASERT COMMENT 'T_OA_WELFAREMASERT';

/*==============================================================*/
/* Table: T_OA_WORKRECORD                                       */
/*==============================================================*/
CREATE TABLE T_OA_WORKRECORD
(
   WORKRECORDID         VARCHAR(50) NOT NULL,
   TITLE                VARCHAR(50) NOT NULL,
   PLANTIME             DATETIME NOT NULL,
   CONTENT              VARCHAR(4000) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (WORKRECORDID)
);

ALTER TABLE T_OA_WORKRECORD COMMENT 'T_OA_WORKRECORD';

/*==============================================================*/
/* Table: T_PF_MODULEDEPENDS                                    */
/*==============================================================*/
CREATE TABLE T_PF_MODULEDEPENDS
(
   DEPENDID             VARCHAR(50) NOT NULL COMMENT '依赖的子系统ID。单个项目可能会有多个依赖项。',
   MODULEID             VARCHAR(50) NOT NULL COMMENT '子项目ID',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (DEPENDID, MODULEID)
);

ALTER TABLE T_PF_MODULEDEPENDS COMMENT '子项目依赖表,指定项目的依赖关系.在加载过程中将会检测其依赖项';

/*==============================================================*/
/* Table: T_PF_MODULEINFO                                       */
/*==============================================================*/
CREATE TABLE T_PF_MODULEINFO
(
   MODULEID             VARCHAR(50) NOT NULL COMMENT '子项目ID',
   MODULECODE           VARCHAR(50) NOT NULL COMMENT '子项目编号',
   MODULENAME           VARCHAR(200) NOT NULL COMMENT '子项目名称',
   MODULETYPE           VARCHAR(500) COMMENT '模块类型，Type全称，包含DLL版本信息
            EG:
            TopNamespace.SubNameSpace.ContainingClass+NestedClass, MyAssembly, Version=1.3.0.0, Culture=neutral, PublicKeyToken=b17a5c561934e089',
   PARENTMODULEID       VARCHAR(50) COMMENT '所属父模块ID，可以为空',
   MODULEICON           VARCHAR(200) COMMENT '图标地址',
   VERSION              VARCHAR(500) NOT NULL COMMENT '当前版本号，版本号与项目部署文件有关系。',
   FILENAME             VARCHAR(200) COMMENT '文件名称',
   FILEPATH             VARCHAR(200) COMMENT '文件路径',
   ENTERASSEMBLY        VARCHAR(200) COMMENT '入口程序集',
   ISSAVE               VARCHAR(1) DEFAULT '1' COMMENT '是否持久化 0:不进行持久化   1:持久化',
   CLIENTID             VARCHAR(200) COMMENT '客户端ID',
   SERVERID             VARCHAR(200) COMMENT '服务端ID',
   USESTATE             VARCHAR(1) DEFAULT '0' COMMENT '使用状态 0:未启用 1:启用',
   HOSTADDRESS          VARCHAR(200) COMMENT '项目服务主机地址',
   DESCRIPTION          VARCHAR(2000) COMMENT '当前版本描述信息。更新后此信息将写入历史记录。',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (MODULEID)
);

ALTER TABLE T_PF_MODULEINFO COMMENT '平台包含的模块信息配置表。仅有超级管理员有此系统权限';

/*==============================================================*/
/* Table: T_PF_MODULEUPDATELOG                                  */
/*==============================================================*/
CREATE TABLE T_PF_MODULEUPDATELOG
(
   MODULELOGID          VARCHAR(50) NOT NULL COMMENT '更新记录ID',
   MODULEID             VARCHAR(50) COMMENT '子项目ID',
   VERSION              VARCHAR(50) COMMENT '版本ID，子项目的更新版本。版本号与项目部署文件有关系。',
   DESCRIPTION          VARCHAR(2000) COMMENT '版本描述信息。对当前版本更新内容进行描述。',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (MODULELOGID)
);

ALTER TABLE T_PF_MODULEUPDATELOG COMMENT '子系统更新记录，用于记录当前系统的更新信息，此记录可以用于查看系统的历史版本和更新信息。';

/*==============================================================*/
/* Table: T_PF_MODULE_RESOURCE                                  */
/*==============================================================*/
CREATE TABLE T_PF_MODULE_RESOURCE
(
   MODULEINFOID         VARCHAR(50) NOT NULL COMMENT '主键ID',
   RESOURCEID           VARCHAR(50) COMMENT '资源ID',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (MODULEINFOID)
);

ALTER TABLE T_PF_MODULE_RESOURCE COMMENT '系统-资源关系表';

/*==============================================================*/
/* Table: T_PF_PUBLICPART                                       */
/*==============================================================*/
CREATE TABLE T_PF_PUBLICPART
(
   PARTID               VARCHAR(50) NOT NULL COMMENT '组件ID',
   MODULEID             VARCHAR(50) COMMENT '子项目ID',
   PARTKEY              VARCHAR(50) COMMENT '唯一',
   PIOCPATH             VARCHAR(200) COMMENT '图标路径',
   TITEL                VARCHAR(200) NOT NULL COMMENT '标题',
   FULLNAME             VARCHAR(200) NOT NULL COMMENT '完整路径',
   ASSEMPLYNAME         VARCHAR(200) NOT NULL COMMENT '所属程序集名',
   PARAMS               VARCHAR(500) COMMENT '初始参数',
   SMTPFTATE            VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '使用状态  0: 未启用 1:  启用',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (PARTID)
);

ALTER TABLE T_PF_PUBLICPART COMMENT '系统公共组件表。提供给系统间的共享调用模块。';

/*==============================================================*/
/* Table: T_PF_RESOURCE                                         */
/*==============================================================*/
CREATE TABLE T_PF_RESOURCE
(
   RESOURCEID           VARCHAR(50) NOT NULL COMMENT '资源ID',
   RESOURCENAME         VARCHAR(50) COMMENT '资源名称',
   FILEPATH             VARCHAR(50) COMMENT '资源路径',
   VERSION              VARCHAR(50) COMMENT '资源版本',
   TYPE                 VARCHAR(50) COMMENT '资源类型',
   STATE                VARCHAR(50) DEFAULT '0' COMMENT '资源状态
            0:未启用
            1:启用',
   DESCRIPTION          VARCHAR(2000) COMMENT '资源描述',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (RESOURCEID)
);

ALTER TABLE T_PF_RESOURCE COMMENT '系统资源管理表';

/*==============================================================*/
/* Table: T_PF_RESOURCEHOST                                     */
/*==============================================================*/
CREATE TABLE T_PF_RESOURCEHOST
(
   HOSTID               VARCHAR(50) NOT NULL COMMENT '主机ID,唯一,主键',
   HOSTNAME             VARCHAR(50) COMMENT '资源主机名称',
   HOSTADDRESS          VARCHAR(200) COMMENT '主机地址，用于根据主机地址请求同步',
   DESCRIPTION          VARCHAR(500) COMMENT '描述信息，对主机情况的概要描述',
   HOSTVERSION          VARCHAR(50) COMMENT '主机版本,描述当前的主机版本.',
   ISMAINHOST           VARCHAR(1) DEFAULT '0' COMMENT '主服务器，如果为主服务器，其他服务器可以请求主服务器进行资源同步。
            0：非主服务器，默认
            1：主服务器',
   ISACCESS             VARCHAR(1) DEFAULT '0' COMMENT '主机是否开放给外网访问，当可访问状态下，主服务器可以通知子服务器进行更新。
            0：不可访问，默认
            1：可访问',
   SYNCDATE             DATETIME COMMENT '最后同步时间。描述主服务器与子服务器的更新时间差异。',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (HOSTID)
);

ALTER TABLE T_PF_RESOURCEHOST COMMENT '资源服务器主机列表，用于记录分布式中涉及到的主机信息。便于主从服务器的同步更新。';

/*==============================================================*/
/* Table: T_PF_RESOURCEMAPPING                                  */
/*==============================================================*/
CREATE TABLE T_PF_RESOURCEMAPPING
(
   MAPPINGID            VARCHAR(50) NOT NULL,
   HOSTID               VARCHAR(50) COMMENT '主机ID,唯一,主键',
   STARTIP              VARCHAR(50),
   ENDIP                VARCHAR(50),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (MAPPINGID)
);

ALTER TABLE T_PF_RESOURCEMAPPING COMMENT '资源服务映射访问范围,单个资源服务器可以对应多个可访问的IP范围，若用户IP不存在与记录中，则默认指向主资源服务器';

/*==============================================================*/
/* Table: T_PF_SHORTCUT                                         */
/*==============================================================*/
CREATE TABLE T_PF_SHORTCUT
(
   SHORTCUTID           VARCHAR(50) NOT NULL COMMENT '快捷键ID',
   MODULEID             VARCHAR(50) COMMENT '子项目ID',
   TITEL                VARCHAR(200) NOT NULL COMMENT '标题',
   ASSEMPLYNAME         VARCHAR(200) NOT NULL COMMENT '所属程序集名',
   ICONPATH             VARCHAR(200) COMMENT '图标路径',
   FULLNAME             VARCHAR(200) NOT NULL COMMENT '完整路径',
   ISSYSNEED            VARCHAR(1) DEFAULT '0' COMMENT '是否可删除
            0:不可以，1：可以',
   PARAMS               VARCHAR(500) COMMENT '初始参数',
   USERSTATE            VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '使用状态
            0:未启用  1:启用',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (SHORTCUTID)
);

ALTER TABLE T_PF_SHORTCUT COMMENT '平台快捷方式';

/*==============================================================*/
/* Table: T_PF_SYSTEMCONFIG                                     */
/*==============================================================*/
CREATE TABLE T_PF_SYSTEMCONFIG
(
   USERCONFIGID         VARCHAR(50) NOT NULL COMMENT '配置ID',
   USERID               VARCHAR(50) COMMENT '用户ID',
   CONFIGNAME           VARCHAR(50) COMMENT '配置名',
   CONFIGINFO           VARCHAR(2000) COMMENT '配置信息。使用自定义XML格式描述。',
   CONFIGTYPE           VARCHAR(2) DEFAULT '3' COMMENT '配置类型 0: 系统 1:管理员 2:用户',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (USERCONFIGID)
);

ALTER TABLE T_PF_SYSTEMCONFIG COMMENT '系统配置信息表';

/*==============================================================*/
/* Table: T_PF_SYSTEMERROR                                      */
/*==============================================================*/
CREATE TABLE T_PF_SYSTEMERROR
(
   ERRORID              VARCHAR(50) NOT NULL COMMENT '错误日志ID',
   ERRORCODE            VARCHAR(50) COMMENT '日志编号',
   APPNAME              VARCHAR(50) COMMENT '所属系统',
   MODELNAME            VARCHAR(50) COMMENT '所属模块',
   ERRORTITEL           VARCHAR(200) COMMENT '错误标题',
   CATEGORY             VARCHAR(2) DEFAULT '2' COMMENT '错误类型
            0: 调试
            1: 异常
            2: 消息
            3: 警告',
   PRIORITY             VARCHAR(2) COMMENT '错误等级，严重程度。
            0: 无
            1: 高
            2: 中
            3: 低',
   MESSAGE              VARCHAR(2000) COMMENT '错误详细内容，有可能是自定义异常或系统产生的异常。',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (ERRORID)
);

ALTER TABLE T_PF_SYSTEMERROR COMMENT '系统运行错误日志，用于记录系统运行异常，为用户提供反馈错误，为管理员提供错误查找管理。';

/*==============================================================*/
/* Table: T_PF_USER_SHORTCUT                                    */
/*==============================================================*/
CREATE TABLE T_PF_USER_SHORTCUT
(
   USERSHORTCUTID       VARCHAR(50) NOT NULL COMMENT '主键ID',
   SHORTCUTID           VARCHAR(50) COMMENT '快捷键ID',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (USERSHORTCUTID)
);

ALTER TABLE T_PF_USER_SHORTCUT COMMENT '用户快捷方式配置表，记录用户配置，定义的快捷键。';

/*==============================================================*/
/* Table: T_PF_USER_WEBPART                                     */
/*==============================================================*/
CREATE TABLE T_PF_USER_WEBPART
(
   USERWEBPARTID        VARCHAR(50) NOT NULL COMMENT '主键ID',
   WEBPARTID            VARCHAR(50) COMMENT 'WebPartID',
   USERID               VARCHAR(50) NOT NULL COMMENT '用户ID',
   TEMPLATENAME         VARCHAR(200) NOT NULL COMMENT '用户配置模板',
   PARAMS               VARCHAR(500) COMMENT '用户配置参数',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (USERWEBPARTID)
);

ALTER TABLE T_PF_USER_WEBPART COMMENT '用户WEBPART配置表';

/*==============================================================*/
/* Table: T_PF_WEBPART                                          */
/*==============================================================*/
CREATE TABLE T_PF_WEBPART
(
   WEBPARTID            VARCHAR(50) NOT NULL COMMENT 'WebPartID',
   MODULEID             VARCHAR(50) COMMENT '子项目ID',
   ICONPATH             VARCHAR(200) COMMENT '图标路径',
   TITEL                VARCHAR(200) NOT NULL COMMENT '标题',
   FULLNAME             VARCHAR(200) NOT NULL COMMENT '完整路径',
   ASSEMPLYNAME         VARCHAR(200) NOT NULL COMMENT '所属程序集名',
   ISSYSNEED            VARCHAR(1) DEFAULT '0' COMMENT '是否可删除 0:不可以，1：可以',
   PARAMS               VARCHAR(500) COMMENT '初始参数',
   TEMPLATENAME         VARCHAR(200) NOT NULL COMMENT '默认模版',
   OWNERID              VARCHAR(50) NOT NULL,
   SMTPFTATE            VARCHAR(1) NOT NULL DEFAULT '0' COMMENT '0: 未启用 1: 启用',
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (WEBPARTID)
);

ALTER TABLE T_PF_WEBPART COMMENT '平台WEBPART信息表';

/*==============================================================*/
/* Table: T_SYS_AREACITY                                        */
/*==============================================================*/
CREATE TABLE T_SYS_AREACITY
(
   CITYID               NUMERIC(38,0) NOT NULL,
   PROVINCEID           NUMERIC(38,0) NOT NULL,
   CITYNAME             VARCHAR(50),
   ZIPCODE              VARCHAR(50)
);

ALTER TABLE T_SYS_AREACITY COMMENT 'T_SYS_AREACITY';

/*==============================================================*/
/* Table: T_SYS_AREADISTRICT                                    */
/*==============================================================*/
CREATE TABLE T_SYS_AREADISTRICT
(
   DISTRICTID           NUMERIC(38,0) NOT NULL,
   CITYID               NUMERIC(38,0) NOT NULL,
   DISTRICTNAME         VARCHAR(50)
);

ALTER TABLE T_SYS_AREADISTRICT COMMENT 'T_SYS_AREADISTRICT';

/*==============================================================*/
/* Table: T_SYS_AREAPROVINCE                                    */
/*==============================================================*/
CREATE TABLE T_SYS_AREAPROVINCE
(
   PROVINCEID           NUMERIC(38,0) NOT NULL,
   PROVINCENAME         VARCHAR(50),
   COUNTRYID            NUMERIC(38,0)
);

ALTER TABLE T_SYS_AREAPROVINCE COMMENT 'T_SYS_AREAPROVINCE';

/*==============================================================*/
/* Table: T_SYS_COUNTRY                                         */
/*==============================================================*/
CREATE TABLE T_SYS_COUNTRY
(
   COUNTRYID            VARCHAR(50) NOT NULL COMMENT '国家ID',
   COUNTRYNAME          VARCHAR(50) COMMENT '国家名称',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (COUNTRYID)
);

ALTER TABLE T_SYS_COUNTRY COMMENT '国家';

/*==============================================================*/
/* Table: T_SYS_DICTIONARY                                      */
/*==============================================================*/
CREATE TABLE T_SYS_DICTIONARY
(
   DICTIONARYID         VARCHAR(50) NOT NULL COMMENT '字典ID',
   DICTIONCATEGORY      VARCHAR(50) COMMENT '字典类型',
   DICTIONARYNAME       VARCHAR(50) COMMENT '字典名称',
   DICTIONARYVALUE      NUMERIC(8,0) COMMENT '字典值',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   DICTIONCATEGORYNAME  VARCHAR(50) COMMENT '字典类型名',
   REMARK               VARCHAR(2000) COMMENT '备注',
   FATHERID             VARCHAR(50) COMMENT '父字典ID',
   SYSTEMNAME           VARCHAR(100) COMMENT '系统类型名:公共Common,人力资源管理:HR,办公自动化OA,物流LM',
   SYSTEMCODE           VARCHAR(100) COMMENT '系统类型编码',
   ORDERNUMBER          NUMERIC(8,0) COMMENT '排序号',
   SYSTEMNEED           VARCHAR(1) COMMENT '系统必须字典:用户不能修改',
   PRIMARY KEY (DICTIONARYID)
);

ALTER TABLE T_SYS_DICTIONARY COMMENT '系统数据字典';

/*==============================================================*/
/* Table: T_SYS_ENTITYMENUBAK                                   */
/*==============================================================*/
CREATE TABLE T_SYS_ENTITYMENUBAK
(
   ENTITYMENUID         VARCHAR(50) NOT NULL,
   SYSTEMTYPE           VARCHAR(50) NOT NULL,
   ENTITYNAME           VARCHAR(200),
   ENTITYCODE           VARCHAR(200),
   HASSYSTEMMENU        VARCHAR(1),
   SUPERIORID           VARCHAR(50),
   MENUCODE             VARCHAR(50) NOT NULL,
   ORDERNUMBER          NUMERIC(38,0) NOT NULL,
   MENUNAME             VARCHAR(50) NOT NULL,
   MENUICONPATH         VARCHAR(100),
   URLADDRESS           VARCHAR(500),
   REMARK               VARCHAR(2000),
   CREATEUSER           VARCHAR(50),
   CREATEDATE           DATETIME,
   UPDATEUSER           VARCHAR(50),
   UPDATEDATE           DATETIME,
   CHILDSYSTEMNAME      VARCHAR(500)
);

ALTER TABLE T_SYS_ENTITYMENUBAK COMMENT 'T_SYS_ENTITYMENUBAK';

/*==============================================================*/
/* Table: T_SYS_ENTITYMENU                                      */
/*==============================================================*/
CREATE TABLE T_SYS_ENTITYMENU
(
   ENTITYMENUID         VARCHAR(50) NOT NULL COMMENT '实体菜单ID',
   SYSTEMTYPE           VARCHAR(50) NOT NULL COMMENT '系统类型',
   ENTITYNAME           VARCHAR(200) COMMENT '实体名称',
   ENTITYCODE           VARCHAR(200) COMMENT '实体编码',
   HASSYSTEMMENU        VARCHAR(1) COMMENT '是否有系统菜单',
   SUPERIORID           VARCHAR(50) COMMENT '父级菜单ID',
   MENUCODE             VARCHAR(50) NOT NULL COMMENT '菜单编号',
   ORDERNUMBER          NUMERIC(38,0) NOT NULL COMMENT '菜单序号',
   MENUNAME             VARCHAR(50) NOT NULL COMMENT '菜单名称',
   MENUICONPATH         VARCHAR(100) COMMENT '菜单图标地址',
   URLADDRESS           VARCHAR(500) COMMENT '链接地址',
   REMARK               VARCHAR(2000) COMMENT '备注：0 hr  1 oa 2物流 3fb 6 资产 7 权限 8 流程  9引擎 10采购 13日常管理',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   CHILDSYSTEMNAME      VARCHAR(500) COMMENT '子系统名称',
   ISAUTHORITY          VARCHAR(1) DEFAULT '0' COMMENT '是否受权限控制',
   HELPTITLE            VARCHAR(100) COMMENT '帮助标题',
   HELPFILEPATH         VARCHAR(500) COMMENT '帮助文件地址',
   PRIMARY KEY (ENTITYMENUID)
);

ALTER TABLE T_SYS_ENTITYMENU COMMENT '系统实体菜单';

/*==============================================================*/
/* Index: IDX_MENUCODE                                          */
/*==============================================================*/
CREATE INDEX IDX_MENUCODE ON T_SYS_ENTITYMENU
(
   MENUCODE
);

/*==============================================================*/
/* Table: T_SYS_ENTITYMENUCUSTOMPERM                            */
/*==============================================================*/
CREATE TABLE T_SYS_ENTITYMENUCUSTOMPERM
(
   ENTITYMENUCUSTOMPERMID VARCHAR(50) NOT NULL COMMENT '自定义菜单权限ID',
   PERMISSIONID         VARCHAR(50) COMMENT '权限ID',
   ENTITYMENUID         VARCHAR(50) COMMENT '实体菜单ID',
   COMPANYID            VARCHAR(50) COMMENT '公司ID',
   COMPANYNAME          VARCHAR(100) COMMENT '公司名称',
   DEPARTMENTID         VARCHAR(50) COMMENT '部门ID',
   DEPARTMENTNAME       VARCHAR(50) COMMENT '部门名称',
   POSTID               VARCHAR(50) COMMENT '岗位ID',
   POSTNAME             VARCHAR(50) COMMENT '岗位名称',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   STARTDATE            DATETIME COMMENT '开始时间',
   ENDDATE              DATETIME COMMENT '失效时间',
   ROLEID               VARCHAR(50) COMMENT '角色ID',
   PRIMARY KEY (ENTITYMENUCUSTOMPERMID)
);

ALTER TABLE T_SYS_ENTITYMENUCUSTOMPERM COMMENT '角色与需要权限控制的实体的权限关系
角色对每个实体的增删改查的权限
角色对实体的权限范围：是对岗';

/*==============================================================*/
/* Index: T_ENTROL                                              */
/*==============================================================*/
CREATE INDEX T_ENTROL ON T_SYS_ENTITYMENUCUSTOMPERM
(
   ENTITYMENUID,
   ROLEID
);

/*==============================================================*/
/* Table: T_SYS_FBADMIN                                         */
/*==============================================================*/
CREATE TABLE T_SYS_FBADMIN
(
   FBADMINID            VARCHAR(50) NOT NULL COMMENT '预算管理员ID',
   SYSUSERID            VARCHAR(50) COMMENT '权限用户ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人ID',
   ADDUSERNAME          VARCHAR(50) COMMENT '创建人姓名',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   ISSUPPERADMIN        VARCHAR(50) COMMENT '是否超级管理员 默认1',
   ISCOMPANYADMIN       VARCHAR(50) COMMENT '是否是公司管理员 默认1  (是)  0：不是',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改人姓名',
   ROLENAME             VARCHAR(50) COMMENT '公司简称+"隐藏预算管理员"',
   REMARK               VARCHAR(200) COMMENT '备注',
   EMPLOYEEID           VARCHAR(50) COMMENT '被授权员工ID',
   EMPLOYEECOMPANYID    VARCHAR(50) COMMENT '被授权员工所在公司ID(考虑兼职)',
   EMPLOYEEDEPARTMENTID VARCHAR(50) COMMENT '被授权员工所在部门ID(考虑兼职)',
   EMPLOYEEPOSTID       VARCHAR(50) COMMENT '被授权员工所在岗位ID(考虑兼职)',
   PRIMARY KEY (FBADMINID)
);

ALTER TABLE T_SYS_FBADMIN COMMENT '预算管理员';

/*==============================================================*/
/* Table: T_SYS_FBADMINROLE                                     */
/*==============================================================*/
CREATE TABLE T_SYS_FBADMINROLE
(
   FBADMINROLEID        VARCHAR(50) NOT NULL COMMENT '管理员角色ID',
   FBADMINID            VARCHAR(50) COMMENT '预算管理员ID',
   ROLEID               VARCHAR(50) COMMENT '角色ID',
   ADDDATE              DATETIME COMMENT '添加时间',
   PRIMARY KEY (FBADMINROLEID)
);

ALTER TABLE T_SYS_FBADMINROLE COMMENT '预算管理员和角色关联表';

/*==============================================================*/
/* Table: T_SYS_FILEUPLOAD                                      */
/*==============================================================*/
CREATE TABLE T_SYS_FILEUPLOAD
(
   FILEUPLOADID         VARCHAR(50) NOT NULL,
   SYSTEMCODE           VARCHAR(50) NOT NULL,
   COMPANYID            VARCHAR(50) NOT NULL,
   MODELNAME            VARCHAR(50) NOT NULL,
   FORMID               VARCHAR(50) NOT NULL,
   FILENAME             VARCHAR(2000) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (FILEUPLOADID)
);

ALTER TABLE T_SYS_FILEUPLOAD COMMENT 'T_SYS_FILEUPLOAD';

/*==============================================================*/
/* Index: IDX_FORMID_FILEUPLOAD                                 */
/*==============================================================*/
CREATE INDEX IDX_FORMID_FILEUPLOAD ON T_SYS_FILEUPLOAD
(
   FORMID
);

/*==============================================================*/
/* Table: T_SYS_PROVINCECITY                                    */
/*==============================================================*/
CREATE TABLE T_SYS_PROVINCECITY
(
   PROVINCEID           VARCHAR(50) NOT NULL COMMENT '省市ID',
   AREANAME             VARCHAR(50) COMMENT '名称',
   COUNTRYID            VARCHAR(50) COMMENT '所属国家',
   FATHERID             VARCHAR(50) COMMENT '父ID',
   ISPROVINCE           VARCHAR(32) COMMENT '是否省',
   ISCITY               VARCHAR(32) COMMENT '是否市',
   ISCOUNTRYTOWN        VARCHAR(32) COMMENT '是否县',
   AREAVALUE            NUMERIC(38,0) COMMENT '城市值',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (PROVINCEID)
);

ALTER TABLE T_SYS_PROVINCECITY COMMENT '主要记录省市、县信息';

/*==============================================================*/
/* Table: T_SYS_PERMISSION                                      */
/*==============================================================*/
CREATE TABLE T_SYS_PERMISSION
(
   PERMISSIONID         VARCHAR(50) NOT NULL COMMENT '权限ID',
   PERMISSIONNAME       VARCHAR(50) NOT NULL COMMENT '权限名称',
   PERMISSIONVALUE      VARCHAR(50) COMMENT '权限值',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   ENTITYMENUID         VARCHAR(50) COMMENT '实体菜单ID',
   ISCOMMOM             VARCHAR(1) COMMENT '是否公共权限项',
   PERMISSIONCODE       VARCHAR(200) COMMENT '权限编码',
   PRIMARY KEY (PERMISSIONID)
);

ALTER TABLE T_SYS_PERMISSION COMMENT '操作权限
增,删,改,查,导入,导出,报表,打印,共享数据....';

/*==============================================================*/
/* Table: T_SYS_REPORTMANAGE                                    */
/*==============================================================*/
CREATE TABLE T_SYS_REPORTMANAGE
(
   REPORTID             VARCHAR(50) NOT NULL COMMENT '报表ID',
   REPORTNAME           VARCHAR(50) NOT NULL COMMENT '报表名称',
   CATEGORY             VARCHAR(50) NOT NULL COMMENT '报表分类',
   REMARK               VARCHAR(1000) COMMENT '报表描述',
   REPORTPATH           VARCHAR(200) NOT NULL COMMENT '报表路径',
   REPORTRDLNAME        VARCHAR(200) NOT NULL COMMENT '报表文件名称',
   REPORTSTATE          NUMERIC(38,0) NOT NULL DEFAULT 0 COMMENT '报表状态',
   CREATEDATE           DATETIME COMMENT '创建日期',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   ROLEIDS              VARCHAR(2000) COMMENT '角色IDS',
   ALLOWEXPORT          NUMERIC(38,0) DEFAULT 0 COMMENT '是否允许导出(0 ：不允许 1：允许)',
   MODELCODE            VARCHAR(50) COMMENT '模块代号',
   PRIMARY KEY (REPORTID)
);

ALTER TABLE T_SYS_REPORTMANAGE COMMENT '报表管理';

/*==============================================================*/
/* Table: T_SYS_REPORTPERSON                                    */
/*==============================================================*/
CREATE TABLE T_SYS_REPORTPERSON
(
   CATEGORYID           VARCHAR(50) NOT NULL COMMENT '主键',
   CATEGORYNAME         VARCHAR(50) NOT NULL COMMENT '分类名称',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建时间',
   USERID               VARCHAR(50) NOT NULL COMMENT '所属人',
   USERNAME             VARCHAR(50) COMMENT '所属人姓名',
   ISREPORT             NUMERIC(38,0) COMMENT '是否是文件夹(0:否 1：是)',
   REPORTID             VARCHAR(50) COMMENT '报表ID',
   REPORTNAME           VARCHAR(50) COMMENT '报表名称',
   PARENTID             VARCHAR(50) COMMENT 'ParentID',
   PRIMARY KEY (CATEGORYID)
);

ALTER TABLE T_SYS_REPORTPERSON COMMENT '个人报表';

/*==============================================================*/
/* Table: T_SYS_REPORTROLE                                      */
/*==============================================================*/
CREATE TABLE T_SYS_REPORTROLE
(
   REPORTROLEID         VARCHAR(50) NOT NULL COMMENT '主键',
   REPORTID             VARCHAR(50) NOT NULL COMMENT '报表ID',
   ROLEID               VARCHAR(50) NOT NULL COMMENT '角色ID',
   ROLENAME             VARCHAR(100) NOT NULL COMMENT '角色名称',
   CREATEDATE           DATETIME NOT NULL COMMENT '创建日期',
   COMPANYID            VARCHAR(50),
   COMPANYNAME          VARCHAR(100),
   PRIMARY KEY (REPORTROLEID)
);

ALTER TABLE T_SYS_REPORTROLE COMMENT 'T_SYS_REPORTROLE';

/*==============================================================*/
/* Table: T_SYS_ROLE_BAK                                        */
/*==============================================================*/
CREATE TABLE T_SYS_ROLE_BAK
(
   ROLEID               VARCHAR(50) NOT NULL,
   SYSTEMTYPE           VARCHAR(50),
   ROLENAME             VARCHAR(50),
   REMARK               VARCHAR(2000),
   CREATEUSER           VARCHAR(50),
   CREATEDATE           DATETIME,
   UPDATEUSER           VARCHAR(50),
   UPDATEDATE           DATETIME,
   OWNERCOMPANYID       VARCHAR(50),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50)
);

ALTER TABLE T_SYS_ROLE_BAK COMMENT 'T_SYS_ROLE_BAK';

/*==============================================================*/
/* Table: T_SYS_RTF                                             */
/*==============================================================*/
CREATE TABLE T_SYS_RTF
(
   RTFID                VARCHAR(50) NOT NULL,
   SYSTEMCODE           VARCHAR(50) NOT NULL,
   COMPANYID            VARCHAR(50) NOT NULL,
   MODELNAME            VARCHAR(50) NOT NULL,
   FORMID               VARCHAR(50) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   CONTENT              LONGBLOB,
   PRIMARY KEY (RTFID)
);

ALTER TABLE T_SYS_RTF COMMENT 'T_SYS_RTF';

/*==============================================================*/
/* Index: IDX_RTFFORID                                          */
/*==============================================================*/
CREATE INDEX IDX_RTFFORID ON T_SYS_RTF
(
   FORMID
);

/*==============================================================*/
/* Table: T_SYS_ROLE                                            */
/*==============================================================*/
CREATE TABLE T_SYS_ROLE
(
   ROLEID               VARCHAR(50) NOT NULL COMMENT '角色ID',
   SYSTEMTYPE           VARCHAR(50) COMMENT '系统类型',
   ROLENAME             VARCHAR(50) COMMENT '角色名称',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人名',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建部门ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建岗位ID',
   CHECKSTATE           VARCHAR(50) COMMENT '审核状态：  0 未提交  1 审核中  2 审核通过  3 审核不通过',
   ISAUTHORY            VARCHAR(50) COMMENT '是否受权限控制 默认为 ：0    、0  不受权限控制    1 受权限控制',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改用户名',
   PRIMARY KEY (ROLEID)
);

ALTER TABLE T_SYS_ROLE COMMENT '角色';

/*==============================================================*/
/* Table: T_SYS_ROLEENTITYMENU                                  */
/*==============================================================*/
CREATE TABLE T_SYS_ROLEENTITYMENU
(
   ROLEENTITYMENUID     VARCHAR(50) NOT NULL COMMENT '角色实体菜单ID',
   ROLEID               VARCHAR(50) NOT NULL COMMENT '角色ID',
   ENTITYMENUID         VARCHAR(50) NOT NULL COMMENT '实体菜单ID',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (ROLEENTITYMENUID)
);

ALTER TABLE T_SYS_ROLEENTITYMENU COMMENT '角色实体菜单';

/*==============================================================*/
/* Table: T_SYS_ROLEMENUPERMISSION                              */
/*==============================================================*/
CREATE TABLE T_SYS_ROLEMENUPERMISSION
(
   ROLEMENUPERMID       VARCHAR(50) NOT NULL COMMENT '角色菜单权限ID',
   PERMISSIONID         VARCHAR(50) COMMENT '权限ID',
   ROLEENTITYMENUID     VARCHAR(50) COMMENT '角色实体菜单ID',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   EXTENDVALUE          VARCHAR(50) COMMENT '扩展值',
   DATARANGE            VARCHAR(50) COMMENT '数据范围',
   PRIMARY KEY (ROLEMENUPERMID)
);

ALTER TABLE T_SYS_ROLEMENUPERMISSION COMMENT '角色菜单权限';

/*==============================================================*/
/* Table: T_SYS_USER                                            */
/*==============================================================*/
CREATE TABLE T_SYS_USER
(
   SYSUSERID            VARCHAR(50) NOT NULL COMMENT '系统用户ID',
   EMPLOYEEID           VARCHAR(50) COMMENT '员工ID',
   EMPLOYEENAME         VARCHAR(50) COMMENT '员工姓名',
   EMPLOYEECODE         VARCHAR(50) COMMENT '员工编号',
   USERNAME             VARCHAR(50) NOT NULL COMMENT '员工系统帐号',
   PASSWORD             VARCHAR(50) NOT NULL COMMENT '用户密码',
   STATE                VARCHAR(50) COMMENT '状态：0 禁用，1 使用',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   ISMANGER             NUMERIC(38,0) COMMENT '是否为管理员',
   ISFLOWMANAGER        VARCHAR(1) DEFAULT '0' COMMENT '是否是流程管理员',
   ISENGINEMANAGER      VARCHAR(1) DEFAULT '0' COMMENT '是否是引擎管理员',
   PRIMARY KEY (SYSUSERID)
);

ALTER TABLE T_SYS_USER COMMENT '系统用户表';

/*==============================================================*/
/* Table: T_SYS_USERACTLOG                                      */
/*==============================================================*/
CREATE TABLE T_SYS_USERACTLOG
(
   LOGID                VARCHAR(50) NOT NULL COMMENT '日志ID',
   CLIENTOS             VARCHAR(200) COMMENT '客户端操作系统',
   CLIENTOSLANGUAGE     VARCHAR(200) COMMENT '客户端语言',
   CLIENTBROWSER        VARCHAR(200) COMMENT '客户端浏览器',
   CLIENTHOSTADDRESS    VARCHAR(200) COMMENT '客户端IP',
   CLIENTNETRUNTIME     VARCHAR(200) COMMENT '客户端Runtime',
   ENTITYMENU           VARCHAR(500) COMMENT '所属功能菜单',
   LOGTYPE              VARCHAR(2) COMMENT '0,行为日志
            1,数据日志',
   LOGCONTEXT           VARCHAR(2000) COMMENT '日志内容',
   SERVEROS             VARCHAR(50) COMMENT '服务端操作系统',
   SERVERNETRUNTIME     VARCHAR(2000) COMMENT '服务器Runtime',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   CREATEDATE           DATETIME COMMENT '创建时间',
   COMPANYNAME          VARCHAR(200) COMMENT '公司名称',
   DEPARTMENTNAME       VARCHAR(200) COMMENT '部门名称',
   POSTNAME             VARCHAR(200) COMMENT '岗位名称',
   EMPLOYEENAME         VARCHAR(200) COMMENT '员工名称',
   PRIMARY KEY (LOGID)
);

ALTER TABLE T_SYS_USERACTLOG COMMENT '用户行为日志表';

/*==============================================================*/
/* Table: T_SYS_USERDATALOG                                     */
/*==============================================================*/
CREATE TABLE T_SYS_USERDATALOG
(
   LOGID                VARCHAR(50) NOT NULL COMMENT '日志ID',
   CLIENTOS             VARCHAR(200) COMMENT '客户端操作系统',
   CLIENTOSLANGUAGE     VARCHAR(200) COMMENT '客户端语言',
   CLIENTBROWSER        VARCHAR(200) COMMENT '客户端浏览器',
   CLIENTHOSTADDRESS    VARCHAR(200) COMMENT '客户端IP',
   CLIENTNETRUNTIME     VARCHAR(200) COMMENT '客户端Runtime',
   ENTITYMENU           VARCHAR(500) COMMENT '所属功能菜单',
   LOGTYPE              VARCHAR(2) COMMENT '0,行为日志
            1,数据日志',
   LOGCONTEXT           VARCHAR(2000) COMMENT '日志内容',
   SERVEROS             VARCHAR(50) COMMENT '服务端操作系统',
   SERVERNETRUNTIME     VARCHAR(2000) COMMENT '服务器Runtime',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   CREATEDATE           DATETIME COMMENT '创建时间',
   COMPANYNAME          VARCHAR(200) COMMENT '公司名称',
   DEPARTMENTNAME       VARCHAR(200) COMMENT '部门名称',
   POSTNAME             VARCHAR(200) COMMENT '岗位名称',
   EMPLOYEENAME         VARCHAR(200) COMMENT '员工名称',
   PRIMARY KEY (LOGID)
);

ALTER TABLE T_SYS_USERDATALOG COMMENT '用户数据日志表';

/*==============================================================*/
/* Table: T_SYS_USERLOGINRECORD                                 */
/*==============================================================*/
CREATE TABLE T_SYS_USERLOGINRECORD
(
   LOGINRECORDID        VARCHAR(50) NOT NULL COMMENT '登录系统记录ID',
   USERNAME             VARCHAR(50) NOT NULL COMMENT '员工系统帐号',
   LOGINDATE            DATETIME COMMENT '登录日期',
   LOGINTIME            VARCHAR(50) COMMENT '登录时间',
   LOGINIP              VARCHAR(50) COMMENT '登录IP',
   ONLINESTATE          VARCHAR(1) COMMENT '在线状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属员工岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属员工部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属员工公司ID',
   OWNERNAME            VARCHAR(50) COMMENT '所属员工姓名',
   OWNERPOSTNAME        VARCHAR(50) COMMENT '所属员工岗位名称',
   OWNERCOMPANYNAME     VARCHAR(50) COMMENT '所属员工部门名称',
   OWNERDEPARTMENTNAME  VARCHAR(50) COMMENT '所属员工公司名称',
   LOGINYEAR            NUMERIC(8,0) COMMENT '登录年份',
   LOGINMONTH           NUMERIC(8,0) COMMENT '登录月份',
   PRIMARY KEY (LOGINRECORDID)
);

ALTER TABLE T_SYS_USERLOGINRECORD COMMENT '用户登录系统记录表';

/*==============================================================*/
/* Index: IDX_CORDLOGINYEAR                                     */
/*==============================================================*/
CREATE INDEX IDX_CORDLOGINYEAR ON T_SYS_USERLOGINRECORD
(
   LOGINYEAR
);

/*==============================================================*/
/* Table: T_SYS_USERLOGINRECORDHIS                              */
/*==============================================================*/
CREATE TABLE T_SYS_USERLOGINRECORDHIS
(
   LOGINRECORDHISID     VARCHAR(50) NOT NULL COMMENT '登录系统记录ID',
   USERNAME             VARCHAR(50) NOT NULL COMMENT '员工系统帐号',
   LOGINDATE            DATETIME COMMENT '登录日期',
   LOGINTIME            VARCHAR(50) COMMENT '登录时间',
   LOGINIP              VARCHAR(50) COMMENT '登录IP',
   ONLINESTATE          VARCHAR(1) COMMENT '在线状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   PRIMARY KEY (LOGINRECORDHISID)
);

ALTER TABLE T_SYS_USERLOGINRECORDHIS COMMENT '用户登录系统历史记录';

/*==============================================================*/
/* Table: T_SYS_USERROLE                                        */
/*==============================================================*/
CREATE TABLE T_SYS_USERROLE
(
   USERROLEID           VARCHAR(50) NOT NULL COMMENT '用户角色ID',
   ROLEID               VARCHAR(50) COMMENT '角色ID',
   SYSUSERID            VARCHAR(50) COMMENT '系统用户ID',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   POSTID               VARCHAR(50) COMMENT '岗位ID',
   EMPLOYEEPOSTID       VARCHAR(50) COMMENT '员工岗位id',
   PRIMARY KEY (USERROLEID)
);

ALTER TABLE T_SYS_USERROLE COMMENT '用户角色';

/*==============================================================*/
/* Table: T_WF_AUDITSCOPE                                       */
/*==============================================================*/
CREATE TABLE T_WF_AUDITSCOPE
(
   SCOPEID              VARCHAR(50) NOT NULL,
   COMPANYID            VARCHAR(50),
   COMPANYNAME          VARCHAR(50),
   CREATEDATE           DATETIME,
   CREATEUSERID         VARCHAR(50),
   CREATEUSER           VARCHAR(50),
   PRIMARY KEY (SCOPEID)
);

ALTER TABLE T_WF_AUDITSCOPE COMMENT 'T_WF_AUDITSCOPE';

/*==============================================================*/
/* Table: T_WF_DEFAULTMESSAGE                                   */
/*==============================================================*/
CREATE TABLE T_WF_DEFAULTMESSAGE
(
   MESSAGEID            VARCHAR(50) NOT NULL COMMENT '消息ID(GUID)',
   SYSTEMCODE           VARCHAR(50) COMMENT '系统代码',
   SYSTEMNAME           VARCHAR(50) COMMENT '系统名称',
   MODELCODE            VARCHAR(50) COMMENT '模块代码',
   MODELNAME            VARCHAR(50) COMMENT '模块名称',
   APPLICATIONURL       VARCHAR(2000) COMMENT '应用URL',
   AUDITSTATE           NUMERIC(8,0) COMMENT '审核状态:1审核中,2审核通过,3审核不通过',
   MESSAGECONTENT       VARCHAR(200) COMMENT '消息内容',
   CREATEDATE           DATETIME  COMMENT '创建日期时间',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人ID',
   CREATEUSERNAME       VARCHAR(50) COMMENT '他建人姓名',
   UPDATEUSERID         VARCHAR(50) COMMENT '修改人ID',
   UPDATEUSERNAME       VARCHAR(50) COMMENT '修改人姓名',
   UPDATEDATE           DATETIME COMMENT '修改日期时间',
   LASTDAYS             NUMERIC(8,0) DEFAULT 3 COMMENT '可处理日期（剩余天数）'
);

ALTER TABLE T_WF_DEFAULTMESSAGE COMMENT '全局默认消息,消息规则没有定义时从这个表取值发送';

/*==============================================================*/
/* Table: T_WF_DOTASK                                           */
/*==============================================================*/
CREATE TABLE T_WF_DOTASK
(
   DOTASKID             VARCHAR(50) NOT NULL COMMENT '待办任务ID',
   COMPANYID            VARCHAR(50) COMMENT '公司ID',
   ORDERID              VARCHAR(50) COMMENT '单据ID',
   ORDERUSERID          VARCHAR(50) COMMENT '单据所属人ID',
   ORDERUSERNAME        VARCHAR(50) COMMENT '单据所属人名称',
   ORDERSTATUS          NUMERIC(8,0) COMMENT '单据状态',
   MESSAGEBODY          VARCHAR(2000) COMMENT '消息体',
   APPLICATIONURL       VARCHAR(2000) COMMENT '应用URL',
   RECEIVEUSERID        VARCHAR(50) COMMENT '接收用户ID',
   BEFOREPROCESSDATE    DATETIME COMMENT '可处理时间（主要针对KPI考核）',
   DOTASKTYPE           NUMERIC(8,0) COMMENT '待办任务类型(0、待办任务、1、流程咨询、3 )',
   CLOSEDDATE           DATETIME COMMENT '待办关闭时间',
   ENGINECODE           VARCHAR(200) COMMENT '引擎代码',
   DOTASKSTATUS         NUMERIC(8,0) DEFAULT 0 COMMENT '代办任务状态(0、未处理 1、已处理 、2、任务撤销 10、删除)',
   MAILSTATUS           NUMERIC(8,0) DEFAULT 0 COMMENT '邮件状态(0、未发送 1、已发送、2、未知 )',
   RTXSTATUS            NUMERIC(8,0) DEFAULT 0 COMMENT 'RTX状态(0、未发送 1、已发送、2、未知 )',
   ISALARM              NUMERIC(8,0) COMMENT '是否已提醒(0、未提醒 1、已提醒、2、未知 )',
   APPFIELDVALUE        TEXT COMMENT '应用字段值',
   FLOWXML              TEXT COMMENT '流程XML',
   APPXML               TEXT COMMENT '应用XML',
   SYSTEMCODE           VARCHAR(50) COMMENT '系统代码',
   SYSTEMNAME           VARCHAR(50) COMMENT '系统名称',
   MODELCODE            VARCHAR(50) COMMENT '模块代码',
   MODELNAME            VARCHAR(100) COMMENT '模块名称',
   CREATEDATETIME       DATETIME  COMMENT '创建日期',
   REMARK               VARCHAR(200) COMMENT '备注',
   APPFIELDVALUE1       TEXT,
   FLOWXML1             TEXT,
   APPXML1              TEXT,
   WORKCODE             VARCHAR(100) COMMENT '--BPM平台流程待办ID',
   ISVALID              NUMERIC(8,0) NOT NULL DEFAULT 1 COMMENT '--是否有效，默认有效【1】，当代理或委托时如果代理人取消了代理权限，则设置为无效【0】',
   VALIDSTARTIME        DATETIME COMMENT '--代理，委托生效开始时间',
   VALIDENDTIME         DATETIME COMMENT '--代理，委托生效结束时间',
   WORKCODECREATETYPE   NUMERIC(8,0) NOT NULL DEFAULT 1 COMMENT '--待办类型【1:自己的待办】【2：代理待办】【3:委托待办】',
   PRIMARY KEY (DOTASKID)
);

ALTER TABLE T_WF_DOTASK COMMENT '待办任务列表';

/*==============================================================*/
/* Index: IDX_DOTAORDERID                                       */
/*==============================================================*/
CREATE INDEX IDX_DOTAORDERID ON T_WF_DOTASK
(
   ORDERID
);

/*==============================================================*/
/* Index: IDX_RECSTATUS                                         */
/*==============================================================*/
CREATE INDEX IDX_RECSTATUS ON T_WF_DOTASK
(
   RECEIVEUSERID,
   DOTASKSTATUS
);

/*==============================================================*/
/* Table: T_WF_DOTASKBAK                                        */
/*==============================================================*/
CREATE TABLE T_WF_DOTASKBAK
(
   DOTASKID             VARCHAR(50) NOT NULL COMMENT '待办任务ID',
   COMPANYID            VARCHAR(50) COMMENT '公司ID',
   ORDERID              VARCHAR(50) COMMENT '单据ID',
   ORDERUSERID          VARCHAR(50) COMMENT '单据所属人ID',
   ORDERUSERNAME        VARCHAR(50) COMMENT '单据所属人名称',
   ORDERSTATUS          NUMERIC(8,0) COMMENT '单据状态',
   MESSAGEBODY          VARCHAR(2000) COMMENT '消息体',
   APPLICATIONURL       VARCHAR(2000) COMMENT '应用URL',
   RECEIVEUSERID        VARCHAR(50) COMMENT '接收用户ID',
   BEFOREPROCESSDATE    DATETIME COMMENT '可处理时间（主要针对KPI考核）',
   DOTASKTYPE           NUMERIC(8,0) COMMENT '待办任务类型(0、待办任务、1、流程咨询、3 )',
   CLOSEDDATE           DATETIME COMMENT '待办关闭时间',
   ENGINECODE           VARCHAR(200) COMMENT '引擎代码',
   DOTASKSTATUS         NUMERIC(8,0) DEFAULT 0 COMMENT '代办任务状态(0、未处理 1、已处理 、2、任务撤销 10、删除)',
   MAILSTATUS           NUMERIC(8,0) DEFAULT 0 COMMENT '邮件状态(0、未发送 1、已发送、2、未知 )',
   RTXSTATUS            NUMERIC(8,0) DEFAULT 0 COMMENT 'RTX状态(0、未发送 1、已发送、2、未知 )',
   ISALARM              NUMERIC(8,0) COMMENT '是否已提醒(0、未提醒 1、已提醒、2、未知 )',
   APPFIELDVALUE        TEXT COMMENT '应用字段值',
   FLOWXML              TEXT COMMENT '流程XML',
   APPXML               TEXT COMMENT '应用XML',
   SYSTEMCODE           VARCHAR(50) COMMENT '系统代码',
   SYSTEMNAME           VARCHAR(50) COMMENT '系统名称',
   MODELCODE            VARCHAR(50) COMMENT '模块代码',
   MODELNAME            VARCHAR(100) COMMENT '模块名称',
   CREATEDATETIME       DATETIME  COMMENT '创建日期',
   REMARK               VARCHAR(200) COMMENT '备注',
   APPFIELDVALUE1       TEXT,
   FLOWXML1             TEXT,
   APPXML1              TEXT,
   WORKCODE             VARCHAR(100) COMMENT '--BPM平台流程待办ID',
   ISVALID              NUMERIC(8,0) NOT NULL DEFAULT 1 COMMENT '--是否有效，默认有效【1】，当代理或委托时如果代理人取消了代理权限，则设置为无效【0】',
   VALIDSTARTIME        DATETIME COMMENT '--代理，委托生效开始时间',
   VALIDENDTIME         DATETIME COMMENT '--代理，委托生效结束时间',
   WORKCODECREATETYPE   NUMERIC(8,0) NOT NULL DEFAULT 1 COMMENT '--待办类型【1:自己的待办】【2：代理待办】【3:委托待办】',
   PRIMARY KEY (DOTASKID)
);

ALTER TABLE T_WF_DOTASKBAK COMMENT '待办任务列表';

/*==============================================================*/
/* Index: IDX_DOTAORDERIDSS                                     */
/*==============================================================*/
CREATE INDEX IDX_DOTAORDERIDSS ON T_WF_DOTASKBAK
(
   ORDERID
);

/*==============================================================*/
/* Index: IDX_RECSTATUSSS                                       */
/*==============================================================*/
CREATE INDEX IDX_RECSTATUSSS ON T_WF_DOTASKBAK
(
   RECEIVEUSERID,
   DOTASKSTATUS
);

/*==============================================================*/
/* Table: T_WF_DOTASKBAK0801                                    */
/*==============================================================*/
CREATE TABLE T_WF_DOTASKBAK0801
(
   DOTASKID             VARCHAR(50) NOT NULL,
   COMPANYID            VARCHAR(50),
   ORDERID              VARCHAR(50),
   ORDERUSERID          VARCHAR(50),
   ORDERUSERNAME        VARCHAR(50),
   ORDERSTATUS          NUMERIC(8,0),
   MESSAGEBODY          VARCHAR(2000),
   APPLICATIONURL       VARCHAR(2000),
   RECEIVEUSERID        VARCHAR(50),
   BEFOREPROCESSDATE    DATETIME,
   DOTASKTYPE           NUMERIC(8,0),
   CLOSEDDATE           DATETIME,
   ENGINECODE           VARCHAR(200),
   DOTASKSTATUS         NUMERIC(8,0),
   MAILSTATUS           NUMERIC(8,0),
   RTXSTATUS            NUMERIC(8,0),
   ISALARM              NUMERIC(8,0),
   APPFIELDVALUE        TEXT,
   FLOWXML              TEXT,
   APPXML               TEXT,
   SYSTEMCODE           VARCHAR(50),
   SYSTEMNAME           VARCHAR(50),
   MODELCODE            VARCHAR(50),
   MODELNAME            VARCHAR(100),
   CREATEDATETIME       DATETIME,
   REMARK               VARCHAR(200),
   APPFIELDVALUE1       TEXT,
   FLOWXML1             TEXT,
   APPXML1              TEXT
);

ALTER TABLE T_WF_DOTASKBAK0801 COMMENT 'T_WF_DOTASKBAK0801';

/*==============================================================*/
/* Table: T_WF_DOTASKMESSAGE                                    */
/*==============================================================*/
CREATE TABLE T_WF_DOTASKMESSAGE
(
   DOTASKMESSAGEID      VARCHAR(50) NOT NULL COMMENT '待办消息ID',
   MESSAGEBODY          VARCHAR(500) COMMENT '消息体',
   SYSTEMCODE           VARCHAR(50) COMMENT '系统代码',
   RECEIVEUSERID        VARCHAR(50) COMMENT '接收用户',
   ORDERID              VARCHAR(50) COMMENT '单据ID',
   COMPANYID            VARCHAR(50) COMMENT '公司ID',
   MESSAGESTATUS        NUMERIC(8,0) COMMENT '消息状态(0、未处理 1、已处理 、2、消息撤销 )',
   MAILSTATUS           NUMERIC(8,0) COMMENT '邮件状态(0、未发送 1、已发送、2、未知 )',
   RTXSTATUS            NUMERIC(8,0) COMMENT 'TRX状态(0、未发送 1、已发送、2、未知 )',
   CLOSEDDATE           DATETIME COMMENT '消息关闭时间',
   CREATEDATETIME       DATETIME  COMMENT '创建日期时间',
   REMARK               VARCHAR(200) COMMENT '备注',
   PRIMARY KEY (DOTASKMESSAGEID)
);

ALTER TABLE T_WF_DOTASKMESSAGE COMMENT '待办消息列表';

/*==============================================================*/
/* Index: IDX_MESAGED_MAUS                                      */
/*==============================================================*/
CREATE INDEX IDX_MESAGED_MAUS ON T_WF_DOTASKMESSAGE
(
   MAILSTATUS,
   MESSAGESTATUS
);

/*==============================================================*/
/* Table: T_WF_DOTASKRULE                                       */
/*==============================================================*/
CREATE TABLE T_WF_DOTASKRULE
(
   DOTASKRULEID         VARCHAR(100) NOT NULL COMMENT '待办规则主表ID',
   COMPANYID            VARCHAR(50) COMMENT '公司ID',
   SYSTEMCODE           VARCHAR(50) COMMENT '系统代码',
   SYSTEMNAME           VARCHAR(50) COMMENT '系统名称',
   MODELCODE            VARCHAR(50) COMMENT '模块代码',
   MODELNAME            VARCHAR(50) COMMENT '模块名称',
   TRIGGERORDERSTATUS   NUMERIC(8,0) COMMENT '触发条件的单据状态',
   CREATEDATETIME       DATETIME  COMMENT '创建日期时间',
   PRIMARY KEY (DOTASKRULEID)
);

ALTER TABLE T_WF_DOTASKRULE COMMENT '待办任务消息规则主表';

/*==============================================================*/
/* Index: IDX_CODEMODEID                                        */
/*==============================================================*/
CREATE INDEX IDX_CODEMODEID ON T_WF_DOTASKRULE
(
   TRIGGERORDERSTATUS,
   SYSTEMCODE,
   COMPANYID,
   MODELCODE
);

/*==============================================================*/
/* Table: T_WF_DOTASKRULEDETAIL                                 */
/*==============================================================*/
CREATE TABLE T_WF_DOTASKRULEDETAIL
(
   DOTASKRULEDETAILID   VARCHAR(100) NOT NULL COMMENT '待办规则明细ID',
   DOTASKRULEID         VARCHAR(100) NOT NULL COMMENT '待办规则主表ID',
   COMPANYID            VARCHAR(50) COMMENT '公司ID',
   SYSTEMCODE           VARCHAR(50) COMMENT '系统代码',
   SYSTEMNAME           VARCHAR(50) COMMENT '系统名称',
   MODELCODE            VARCHAR(50) COMMENT '模块代码',
   TRIGGERORDERSTATUS   NUMERIC(8,0) COMMENT '触发条件的单据状态',
   MODELNAME            VARCHAR(50) COMMENT '模块名称',
   WCFURL               VARCHAR(200) COMMENT 'WCF的URL',
   FUNCTIONNAME         VARCHAR(50) COMMENT '所调方法名称',
   FUNCTIONPARAMTER     VARCHAR(2000) COMMENT '方法参数',
   PAMETERSPLITCHAR     VARCHAR(100) COMMENT '参数分解符',
   WCFBINDINGCONTRACT   VARCHAR(100) COMMENT 'WCF绑定的契约',
   MESSAGEBODY          VARCHAR(400) COMMENT '消息体',
   LASTDAYS             NUMERIC(8,0) COMMENT '可处理日期（剩余天数）',
   APPLICATIONURL       TEXT COMMENT '应用URL',
   RECEIVEUSER          VARCHAR(100) COMMENT '接收用户',
   RECEIVEUSERNAME      VARCHAR(100) COMMENT '接收用户名',
   OWNERCOMPANYID       VARCHAR(100) COMMENT '所属公司ID',
   OWNERDEPARTMENTID    VARCHAR(100) COMMENT '所属部门ID',
   OWNERPOSTID          VARCHAR(100) COMMENT '所属岗位ID',
   ISDEFAULTMSG         NUMERIC(8,0) COMMENT '是否缺省消息',
   PROCESSFUNCLANGUAGE  VARCHAR(100) COMMENT '处理功能语言',
   ISOTHERSOURCE        VARCHAR(100) COMMENT '是否其它来源',
   OTHERSYSTEMCODE      VARCHAR(100) COMMENT '其它系统代码',
   OTHERMODELCODE       VARCHAR(100) COMMENT '其它系统模块',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人ID',
   CREATEDATETIME       DATETIME  COMMENT '创建日期',
   REMARK               VARCHAR(200) COMMENT '备注',
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATETIME       DATETIME,
   PRIMARY KEY (DOTASKRULEDETAILID)
);

ALTER TABLE T_WF_DOTASKRULEDETAIL COMMENT '待办任务消息规则细表';

/*==============================================================*/
/* Table: T_WF_DOTASKRULEDETAIL_20140521                        */
/*==============================================================*/
CREATE TABLE T_WF_DOTASKRULEDETAIL_20140521
(
   DOTASKRULEDETAILID   VARCHAR(100) NOT NULL,
   DOTASKRULEID         VARCHAR(100) NOT NULL,
   COMPANYID            VARCHAR(50),
   SYSTEMCODE           VARCHAR(50),
   SYSTEMNAME           VARCHAR(50),
   MODELCODE            VARCHAR(50),
   TRIGGERORDERSTATUS   NUMERIC(8,0),
   MODELNAME            VARCHAR(50),
   WCFURL               VARCHAR(200),
   FUNCTIONNAME         VARCHAR(50),
   FUNCTIONPARAMTER     VARCHAR(2000),
   PAMETERSPLITCHAR     VARCHAR(100),
   WCFBINDINGCONTRACT   VARCHAR(100),
   MESSAGEBODY          VARCHAR(400),
   LASTDAYS             NUMERIC(8,0),
   APPLICATIONURL       TEXT,
   RECEIVEUSER          VARCHAR(100),
   RECEIVEUSERNAME      VARCHAR(100),
   OWNERCOMPANYID       VARCHAR(100),
   OWNERDEPARTMENTID    VARCHAR(100),
   OWNERPOSTID          VARCHAR(100),
   ISDEFAULTMSG         NUMERIC(8,0),
   PROCESSFUNCLANGUAGE  VARCHAR(100),
   ISOTHERSOURCE        VARCHAR(100),
   OTHERSYSTEMCODE      VARCHAR(100),
   OTHERMODELCODE       VARCHAR(100),
   CREATEUSERNAME       VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEDATETIME       DATETIME,
   REMARK               VARCHAR(200),
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATETIME       DATETIME
);

ALTER TABLE T_WF_DOTASKRULEDETAIL_20140521 COMMENT 'T_WF_DOTASKRULEDETAIL_20140521';

/*==============================================================*/
/* Table: T_WF_DOTASKRULE_20140521                              */
/*==============================================================*/
CREATE TABLE T_WF_DOTASKRULE_20140521
(
   DOTASKRULEID         VARCHAR(100) NOT NULL,
   COMPANYID            VARCHAR(50),
   SYSTEMCODE           VARCHAR(50),
   SYSTEMNAME           VARCHAR(50),
   MODELCODE            VARCHAR(50),
   MODELNAME            VARCHAR(50),
   TRIGGERORDERSTATUS   NUMERIC(8,0),
   CREATEDATETIME       DATETIME
);

ALTER TABLE T_WF_DOTASKRULE_20140521 COMMENT 'T_WF_DOTASKRULE_20140521';

/*==============================================================*/
/* Table: T_WF_ENGINEPREFIX                                     */
/*==============================================================*/
CREATE TABLE T_WF_ENGINEPREFIX
(
   PREFIXCODE           VARCHAR(100) NOT NULL COMMENT '前缀代码',
   PREFIXNAME           VARCHAR(100) COMMENT '前缀名称',
   DEFAULTBIT           VARCHAR(50) COMMENT 'DEFAULTBIT',
   CURRENTORDER         NUMERIC(10,0) COMMENT '当前顺序',
   ORDERLENGTH          NUMERIC(10,0) COMMENT '顺序长度',
   PRIMARY KEY (PREFIXCODE)
);

ALTER TABLE T_WF_ENGINEPREFIX COMMENT '引擎前缀';

/*==============================================================*/
/* Table: T_WF_FLOW_FORWARDED                                   */
/*==============================================================*/
CREATE TABLE T_WF_FLOW_FORWARDED
(
   FORWARDEDID          VARCHAR(50) NOT NULL COMMENT '主键ID',
   FLOWCODE             VARCHAR(50) NOT NULL COMMENT '流程FLOWCODE',
   FORWARDEDTYPE        VARCHAR(2) NOT NULL DEFAULT '0' COMMENT '转发类型【0：人员转发】【1：岗位转发】',
   FORWARDEDTOUSERS     VARCHAR(2000) COMMENT '转发到的需要的人XML',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人ID',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人姓名',
   CREATECOMPANYID      VARCHAR(50) COMMENT '创建人公司ID',
   CREATEDEPARTMENTID   VARCHAR(50) COMMENT '创建人部门ID',
   CREATEPOSTID         VARCHAR(50) COMMENT '创建人岗位ID',
   CREATEDATE           DATETIME  COMMENT '创建日期',
   EDITUSERID           VARCHAR(50) COMMENT '修改人ID',
   EDITUSERNAME         VARCHAR(50) COMMENT '修改人姓名',
   EDITDATE             DATETIME COMMENT '修改日期',
   CONDITIONEXPRESSION  VARCHAR(500) COMMENT '条件表达式（中文）',
   CONDITIONEXPRESSIONEN VARCHAR(500) COMMENT '条件表达式(字段)',
   PRIMARY KEY (FORWARDEDID)
);

ALTER TABLE T_WF_FLOW_FORWARDED COMMENT '流程审核通过后，根据不同的条件转发';

/*==============================================================*/
/* Table: T_WF_FLOW_FORWARDED140623                             */
/*==============================================================*/
CREATE TABLE T_WF_FLOW_FORWARDED140623
(
   FORWARDEDID          VARCHAR(50) NOT NULL,
   FLOWCODE             VARCHAR(50) NOT NULL,
   FORWARDEDTYPE        VARCHAR(2) NOT NULL,
   FORWARDEDTOUSERS     VARCHAR(2000),
   CREATEUSERID         VARCHAR(50),
   CREATEUSERNAME       VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDATE           DATETIME,
   EDITUSERID           VARCHAR(50),
   EDITUSERNAME         VARCHAR(50),
   EDITDATE             DATETIME,
   CONDITIONEXPRESSION  VARCHAR(500),
   CONDITIONEXPRESSIONEN VARCHAR(500)
);

ALTER TABLE T_WF_FLOW_FORWARDED140623 COMMENT 'T_WF_FLOW_FORWARDED140623';

/*==============================================================*/
/* Table: T_WF_FLOW_FORWARDED_AUDIT                             */
/*==============================================================*/
CREATE TABLE T_WF_FLOW_FORWARDED_AUDIT
(
   FORWARDEDID          VARCHAR(50) NOT NULL,
   FLOWDEFINEID         VARCHAR(50),
   FLOWCODE             VARCHAR(50),
   FORWARDEDTYPE        VARCHAR(2) COMMENT '转发类型【0：人员转发】【1：岗位转发】',
   FORWARDEDTOUSERS     VARCHAR(2000) COMMENT '转发到的需要的人XML',
   CREATEUSERID         VARCHAR(50),
   CREATEUSERNAME       VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDATE           DATETIME,
   EDITUSERID           VARCHAR(50),
   EDITUSERNAME         VARCHAR(50),
   EDITDATE             DATETIME,
   CONDITIONEXPRESSION  VARCHAR(500),
   CONDITIONEXPRESSIONEN VARCHAR(500),
   PRIMARY KEY (FORWARDEDID)
);

ALTER TABLE T_WF_FLOW_FORWARDED_AUDIT COMMENT '流程审核通过后，根据不同的条件转发';

/*==============================================================*/
/* Table: T_WF_FORWARDHISTORY                                   */
/*==============================================================*/
CREATE TABLE T_WF_FORWARDHISTORY
(
   FORWARDHISTORYID     VARCHAR(50) NOT NULL,
   FROMOWNERID          VARCHAR(50),
   FROMOWNERNAME        VARCHAR(50),
   TOOWNERID            VARCHAR(50),
   TOOWNERNAME          VARCHAR(50),
   REMARK               VARCHAR(200),
   PERSONALRECORDID     VARCHAR(50),
   MODELCODE            VARCHAR(50) COMMENT '所属模块代码',
   MODELID              VARCHAR(50) COMMENT '单据ID',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEDATE           DATETIME  COMMENT '修改时间',
   PRIMARY KEY (FORWARDHISTORYID)
);

ALTER TABLE T_WF_FORWARDHISTORY COMMENT 'T_WF_FORWARDHISTORY';

/*==============================================================*/
/* Table: T_WF_MESSAGEBODYDEFINE                                */
/*==============================================================*/
CREATE TABLE T_WF_MESSAGEBODYDEFINE
(
   DEFINEID             VARCHAR(50) NOT NULL COMMENT '默认消息ID',
   COMPANYID            VARCHAR(50) COMMENT '公司ID',
   SYSTEMCODE           VARCHAR(50) COMMENT '系统代号',
   MODELCODE            VARCHAR(50) COMMENT '模块代码',
   MESSAGEBODY          VARCHAR(500) COMMENT '消息体',
   MESSAGEURL           VARCHAR(1000) COMMENT '消息链接',
   MESSAGETYPE          NUMERIC(8,0) DEFAULT 0 COMMENT '消息类型',
   CREATEDATE           DATETIME  COMMENT '创建日期',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人名称',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人',
   RECEIVEPOSTID        VARCHAR(1000) COMMENT '接受岗位ID',
   RECEIVEPOSTNAME      VARCHAR(1000) COMMENT '接受岗位名称',
   RECEIVERUSERID       VARCHAR(1000) COMMENT '接收人ID',
   RECEIVERUSERNAME     VARCHAR(1000) COMMENT '接收人名称',
   RECEIVETYPE          NUMERIC(8,0) DEFAULT 0 COMMENT '接受类型 0、按照角色 1、按照人员',
   PRIMARY KEY (DEFINEID)
);

ALTER TABLE T_WF_MESSAGEBODYDEFINE COMMENT '默认消息配置表（如公文发送，考勤异常批量发送同样的消息标题)';

/*==============================================================*/
/* Table: T_WF_MESSAGECONFIG                                    */
/*==============================================================*/
CREATE TABLE T_WF_MESSAGECONFIG
(
   MESSAGEID            VARCHAR(50),
   SYSTEMCODE           VARCHAR(50),
   SYSTEMNAME           VARCHAR(100),
   MODELCODE            VARCHAR(50),
   MODELNAME            VARCHAR(200),
   ISADMINPAGE          NUMERIC(38,0) COMMENT '是否管理员提单页面，如入职申请，员工转正，这些都不是本人操作，均由管理员操作',
   MESSAGETYPE          NUMERIC(38,0) COMMENT '消息规则类型
            1：普通，只包含时间，单据所有人
            2：包含单号'
);

ALTER TABLE T_WF_MESSAGECONFIG COMMENT '公司信息审核通过后需要初始化消息规则，这里将保持一些特殊规则';

/*==============================================================*/
/* Table: T_WF_MESSAGECONFIG_DETAIL                             */
/*==============================================================*/
CREATE TABLE T_WF_MESSAGECONFIG_DETAIL
(
   SYSTEMCODE           VARCHAR(50),
   MODELCODE            VARCHAR(50),
   FIELDNAME            VARCHAR(50),
   FIELDTYPE            NUMERIC(38,0) COMMENT '1：单号
            2：创建人
            3：单据所属人
            4：创建时间'
);

ALTER TABLE T_WF_MESSAGECONFIG_DETAIL COMMENT 'T_WF_MESSAGECONFIG_DETAIL';

/*==============================================================*/
/* Table: T_WF_MESSAGEDEFINE                                    */
/*==============================================================*/
CREATE TABLE T_WF_MESSAGEDEFINE
(
   MESSAGEDEFINEID      VARCHAR(50) NOT NULL COMMENT '消息定义ID',
   MESSAGEKEY           VARCHAR(50) COMMENT '消息KEY',
   MESSAGEBODY          VARCHAR(400) COMMENT '消息体',
   MESSAGEURL           VARCHAR(2000) COMMENT '消息URL',
   CREATEDATETIME       DATETIME  COMMENT '创建日期时间',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建用户名',
   CREATEUSERID         VARCHAR(50) COMMENT '创建用户ID',
   PRIMARY KEY (MESSAGEDEFINEID)
);

ALTER TABLE T_WF_MESSAGEDEFINE COMMENT '消息定义没有界面直接在数据库中写入';

/*==============================================================*/
/* Table: T_WF_MOBILEFILTER                                     */
/*==============================================================*/
CREATE TABLE T_WF_MOBILEFILTER
(
   SYSTEMCODE           VARCHAR(50) NOT NULL,
   MODELCODE            VARCHAR(100),
   MODELNAME            VARCHAR(200)
);

ALTER TABLE T_WF_MOBILEFILTER COMMENT 'T_WF_MOBILEFILTER';

/*==============================================================*/
/* Table: T_WF_PERSONALRECORD                                   */
/*==============================================================*/
CREATE TABLE T_WF_PERSONALRECORD
(
   PERSONALRECORDID     VARCHAR(50) NOT NULL COMMENT '个人单据ID',
   SYSTYPE              VARCHAR(50) COMMENT '系统类型',
   MODELCODE            VARCHAR(50) COMMENT '所属模块代码',
   MODELID              VARCHAR(50) COMMENT '单据ID',
   CHECKSTATE           NUMERIC(8,0) COMMENT '1审核中 2审核通过 3审核不通过',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CONFIGINFO           VARCHAR(1500) COMMENT '参数配置',
   MODELDESCRIPTION     VARCHAR(1500) COMMENT '单据简要描叙',
   ISFORWARD            NUMERIC(8,0) COMMENT '是否转发(0表示非转发，1表示转发)',
   ISVIEW               NUMERIC(8,0) COMMENT '是否已查看(0表示未查看，1表示已查看)',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (PERSONALRECORDID)
);

ALTER TABLE T_WF_PERSONALRECORD COMMENT 'T_WF_PERSONALRECORD';

/*==============================================================*/
/* Index: IDX_CORDCHECKSTATERID                                 */
/*==============================================================*/
CREATE INDEX IDX_CORDCHECKSTATERID ON T_WF_PERSONALRECORD
(
   OWNERID,
   CHECKSTATE,
   ISFORWARD
);

/*==============================================================*/
/* Index: IDX_SYSTYPECODELIDWARD                                */
/*==============================================================*/
CREATE INDEX IDX_SYSTYPECODELIDWARD ON T_WF_PERSONALRECORD
(
   SYSTYPE,
   MODELCODE,
   MODELID,
   ISFORWARD
);

/*==============================================================*/
/* Table: T_WF_PERSONALRECORD0801                               */
/*==============================================================*/
CREATE TABLE T_WF_PERSONALRECORD0801
(
   PERSONALRECORDID     VARCHAR(50) NOT NULL,
   SYSTYPE              VARCHAR(50),
   MODELCODE            VARCHAR(50),
   MODELID              VARCHAR(50),
   CHECKSTATE           NUMERIC(8,0),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   CONFIGINFO           VARCHAR(1500),
   MODELDESCRIPTION     VARCHAR(1500),
   ISFORWARD            NUMERIC(8,0),
   ISVIEW               NUMERIC(8,0),
   CREATEDATE           DATETIME,
   UPDATEDATE           DATETIME
);

ALTER TABLE T_WF_PERSONALRECORD0801 COMMENT 'T_WF_PERSONALRECORD0801';

/*==============================================================*/
/* Table: T_WF_PORTALSETTING                                    */
/*==============================================================*/
CREATE TABLE T_WF_PORTALSETTING
(
   SYSTEMCODE           VARCHAR(50) COMMENT '系统编码',
   MODELCODE            VARCHAR(50) COMMENT '模块编码',
   PORTALTYPE           VARCHAR(50) COMMENT 'Portal类型'
);

ALTER TABLE T_WF_PORTALSETTING COMMENT '模块所属Portal类型';

/*==============================================================*/
/* Index: INX_POTAL_MODELCODE                                   */
/*==============================================================*/
CREATE INDEX INX_POTAL_MODELCODE ON T_WF_PORTALSETTING
(
   MODELCODE
);

/*==============================================================*/
/* Index: INX_POTAL_SYSTEMCODE                                  */
/*==============================================================*/
CREATE INDEX INX_POTAL_SYSTEMCODE ON T_WF_PORTALSETTING
(
   SYSTEMCODE
);

/*==============================================================*/
/* Table: T_WF_PROCEDUREEXCEPTION                               */
/*==============================================================*/
CREATE TABLE T_WF_PROCEDUREEXCEPTION
(
   PCNAME               VARCHAR(100) COMMENT '存储过程名称',
   SQLCODE              VARCHAR(100) COMMENT '错误代码',
   SQLERRM              VARCHAR(500) COMMENT '错误内容',
   CREATERDATA          DATETIME  COMMENT '产生的时候',
   PCSQL                VARCHAR(2000) COMMENT 'SQL语句'
);

ALTER TABLE T_WF_PROCEDUREEXCEPTION COMMENT '记录存储过程的日志';

/*==============================================================*/
/* Table: T_WF_SMSRECORD                                        */
/*==============================================================*/
CREATE TABLE T_WF_SMSRECORD
(
   SMSRECORD            VARCHAR(50) NOT NULL COMMENT '短信记录ID',
   BATCHNUMBER          VARCHAR(50) COMMENT '记录触发批次',
   COMPANYID            VARCHAR(50) COMMENT '(发送方)公司ID',
   SENDSTATUS           NUMERIC(8,0) COMMENT '发送状态',
   ACCOUNT              VARCHAR(50) COMMENT '短信账号',
   MOBILE               VARCHAR(50) COMMENT '电话号码',
   SENDMESSAGE          VARCHAR(200) COMMENT '发送内容',
   SENDTIME             DATETIME COMMENT '发送时间',
   OWNERID              VARCHAR(50) COMMENT '所属人ID',
   OWNERNAME            VARCHAR(50) COMMENT '所属人名称',
   TASKCOUNT            NUMERIC(8,0) COMMENT '短信数量',
   REMARK               VARCHAR(200) COMMENT '备注',
   RECORDDATE           DATETIME  COMMENT '记录时间',
   PRIMARY KEY (SMSRECORD)
);

ALTER TABLE T_WF_SMSRECORD COMMENT '短信发送记录表';

/*==============================================================*/
/* Table: T_WF_TIMINGTRIGGERACTIVITY                            */
/*==============================================================*/
CREATE TABLE T_WF_TIMINGTRIGGERACTIVITY
(
   TRIGGERID            VARCHAR(50) NOT NULL COMMENT '触发ID',
   BUSINESSID           VARCHAR(50) COMMENT '业务系统使用的标示（主要用于业务系统删除触发的标示）',
   TIMINGCONFIGID       VARCHAR(50) COMMENT '定时触发配置ID（有配置界面的ID）',
   TRIGGERNAME          VARCHAR(100) COMMENT '定时触发名称',
   COMPANYID            VARCHAR(50) COMMENT '公司ID',
   SYSTEMCODE           VARCHAR(50) COMMENT '系统代码',
   SYSTEMNAME           VARCHAR(100) COMMENT '系统名称',
   MODELCODE            VARCHAR(100) COMMENT '模块代码',
   MODELNAME            VARCHAR(100) COMMENT '模块名称',
   TRIGGERACTIVITYTYPE  NUMERIC(8,0) COMMENT '触发活动类型（1、短信活动 2、服务活动）',
   TRIGGERTIME          DATETIME NOT NULL COMMENT '触发时间',
   TRIGGERDATE          NUMERIC(8,0) COMMENT '触发周期的日期(暂时没用)',
   TRIGGERROUND         NUMERIC(8,0) NOT NULL COMMENT '触发周期 0 只触发一次 1 分钟 2小时 3 天 4 月 5年 6周 7未知,8季度',
   WCFURL               VARCHAR(200) COMMENT 'WCF的URL',
   FUNCTIONNAME         VARCHAR(50) COMMENT '所调方法名称',
   FUNCTIONPARAMTER     VARCHAR(2000) COMMENT '方法参数',
   PAMETERSPLITCHAR     VARCHAR(100) COMMENT '参数分解符',
   WCFBINDINGCONTRACT   VARCHAR(100) COMMENT 'WCF绑定的契约',
   RECEIVERUSERID       VARCHAR(50) COMMENT '接收人ID',
   RECEIVEROLE          VARCHAR(50) COMMENT '接受人角色',
   RECEIVERNAME         VARCHAR(100) COMMENT '接收人名称',
   MESSAGEBODY          VARCHAR(1000) COMMENT '接受消息',
   MESSAGEURL           VARCHAR(1000) COMMENT '消息链接',
   TRIGGERSTATUS        NUMERIC(8,0) DEFAULT 0 COMMENT '触发器状态',
   TRIGGERTYPE          VARCHAR(50) COMMENT '触发类型',
   TRIGGERDESCRIPTION   VARCHAR(200) COMMENT '触发描述',
   CONTRACTTYPE         VARCHAR(50) COMMENT '接口类型（引擎，定时接口）',
   CREATEDATETIME       DATETIME  COMMENT '创建日期',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人ID',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人',
   REMARK               VARCHAR(200) COMMENT '备注',
   TRIGGERSTART         DATETIME ,
   TRIGGEREND           DATETIME,
   TRIGGERMULTIPLE      NUMERIC(8,0),
   PRIMARY KEY (TRIGGERID)
);

ALTER TABLE T_WF_TIMINGTRIGGERACTIVITY COMMENT '定时触发活动表（由windows服务进行操作调用）';

/*==============================================================*/
/* Table: T_WF_TIMINGTRIGGERCONFIG                              */
/*==============================================================*/
CREATE TABLE T_WF_TIMINGTRIGGERCONFIG
(
   TIMINGCONFIGID       VARCHAR(50) NOT NULL COMMENT '定时触发配置ID',
   TRIGGERNAME          VARCHAR(100) COMMENT '定时触发名称',
   COMPANYID            VARCHAR(50) COMMENT '公司ID',
   SYSTEMCODE           VARCHAR(50) COMMENT '系统代码',
   SYSTEMNAME           VARCHAR(100) COMMENT '系统名称',
   MODELCODE            VARCHAR(100) COMMENT '模块代码',
   MODELNAME            VARCHAR(100) COMMENT '模块名称',
   TRIGGERACTIVITYTYPE  NUMERIC(8,0) COMMENT '触发活动类型（1、短信活动 2、服务活动）',
   TRIGGERTIME          DATETIME NOT NULL COMMENT '触发时间',
   TRIGGERDATE          NUMERIC(8,0) COMMENT '触发周期的日期(暂时没用)',
   TRIGGERROUND         NUMERIC(8,0) NOT NULL COMMENT '触发周期0 只触发一次 1 分钟 2小时 3 天 4 月 5年 6周 7未知',
   WCFURL               VARCHAR(200) COMMENT 'WCF的URL',
   FUNCTIONNAME         VARCHAR(50) COMMENT '所调方法名称',
   FUNCTIONPARAMTER     VARCHAR(2000) COMMENT '方法参数',
   PAMETERSPLITCHAR     VARCHAR(100) COMMENT '参数分解符',
   WCFBINDINGCONTRACT   VARCHAR(100) COMMENT 'WCF绑定的契约',
   RECEIVERUSERID       VARCHAR(50) COMMENT '接收人ID',
   RECEIVEROLE          VARCHAR(50) COMMENT '接受人角色',
   RECEIVERNAME         VARCHAR(100) COMMENT '接收人名称',
   MESSAGEBODY          VARCHAR(2000) COMMENT '接受消息',
   MESSAGEURL           VARCHAR(1000) COMMENT '消息链接',
   TRIGGERSTATUS        NUMERIC(8,0) COMMENT '触发器状态',
   TRIGGERTYPE          VARCHAR(50) COMMENT '触发类型',
   TRIGGERDESCRIPTION   VARCHAR(200) COMMENT '触发描述',
   CONTRACTTYPE         VARCHAR(50) COMMENT '接口类型（引擎，定时接口）',
   CREATEDATETIME       DATETIME COMMENT '创建日期',
   CREATEUSERID         VARCHAR(50) COMMENT '创建人ID',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人',
   REMARK               VARCHAR(200) COMMENT '备注',
   PRIMARY KEY (TIMINGCONFIGID)
);

ALTER TABLE T_WF_TIMINGTRIGGERCONFIG COMMENT '定时触发消息定义表（有配置界面操作）';

/*==============================================================*/
/* Table: T_WF_TIMINGTRIGGERRECORD                              */
/*==============================================================*/
CREATE TABLE T_WF_TIMINGTRIGGERRECORD
(
   RECORDID             VARCHAR(50) NOT NULL COMMENT '记录ID',
   TRIGGERID            VARCHAR(50) COMMENT '触发ID',
   TRIGGERNAME          VARCHAR(100) COMMENT '定时触发名称',
   COMPANYID            VARCHAR(50) COMMENT '公司ID',
   SYSTEMCODE           VARCHAR(50) COMMENT '系统代码',
   MODELCODE            VARCHAR(100) COMMENT '模块代码',
   MODELNAME            VARCHAR(100) COMMENT '模块名称',
   TRIGGERACTIVITYTYPE  NUMERIC(8,0) COMMENT '触发活动类型（1、短信活动 2、服务活动）',
   TRIGGERTIME          DATETIME COMMENT '触发时间',
   TRIGGERROUND         NUMERIC(8,0) COMMENT '触发周期',
   WCFURL               VARCHAR(1000) COMMENT 'WCF的URL',
   FUNCTIONNAME         VARCHAR(50) COMMENT '所调方法名称',
   FUNCTIONPARAMTER     VARCHAR(2000) COMMENT '方法参数',
   PAMETERSPLITCHAR     VARCHAR(100) COMMENT '参数分解符',
   WCFBINDINGCONTRACT   VARCHAR(100) COMMENT 'WCF绑定的契约',
   RECEIVERUSERID       VARCHAR(50) COMMENT '接收人ID',
   RECEIVEROLE          VARCHAR(50) COMMENT '接受人角色',
   RECEIVERNAME         VARCHAR(100) COMMENT '接收人名称',
   MESSAGEBODY          VARCHAR(2000) COMMENT '接受消息',
   MESSAGEURL           VARCHAR(1000) COMMENT '消息链接',
   TRIGGERSTATUS        NUMERIC(8,0) COMMENT '触发器状态',
   TRIGGERTYPE          VARCHAR(50) COMMENT '触发类型',
   TRIGGERDESCRIPTION   VARCHAR(200) COMMENT '触发描述',
   CONTRACTTYPE         VARCHAR(50) COMMENT '接口类型（引擎，定时接口）',
   CREATEUSERNAME       VARCHAR(50) COMMENT '创建人',
   REMARK               VARCHAR(200) COMMENT '备注',
   RECORDDATE           DATETIME  COMMENT '记录日期',
   PRIMARY KEY (RECORDID)
);

ALTER TABLE T_WF_TIMINGTRIGGERRECORD COMMENT '定时触发器执行记录';

/*==============================================================*/
/* Table: T_HR_BASICTABLE                                       */
/*==============================================================*/
CREATE TABLE T_HR_BASICTABLE
(
   REMARK               VARCHAR(2000),
   OWNERID              VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEDATE           DATETIME ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEDATE           DATETIME 
);

ALTER TABLE T_HR_BASICTABLE COMMENT 't_hr_basicTable';

ALTER TABLE FLOW_FLOWRECORDDETAIL_T ADD CONSTRAINT FLOW_FLOWRECORDDETAIL_T FOREIGN KEY (FLOWRECORDMASTERID)
      REFERENCES FLOW_FLOWRECORDMASTER_T (FLOWRECORDMASTERID) ON UPDATE RESTRICT;

ALTER TABLE FLOW_FLOWTOCATEGORY ADD CONSTRAINT FK_FLOW_TOCATEGORY_1 FOREIGN KEY (FLOWCATEGORYID)
      REFERENCES FLOW_FLOWCATEGORY (FLOWCATEGORYID) ON UPDATE RESTRICT;

ALTER TABLE FLOW_MODELFLOWRELATION_T ADD CONSTRAINT FK_FLOW_MODELFLOWRELATION_T FOREIGN KEY (FLOWCODE)
      REFERENCES FLOW_FLOWDEFINE_T (FLOWCODE) ON UPDATE RESTRICT;

ALTER TABLE FLOW_MODELFLOWRELATION_T ADD CONSTRAINT FK_FLOW_MODELFLOWRELATION_T1 FOREIGN KEY (MODELCODE)
      REFERENCES FLOW_MODELDEFINE_T (MODELCODE) ON UPDATE RESTRICT;

ALTER TABLE T_FB_BORROWAPPLYDETAIL ADD CONSTRAINT FK_REFERENCE_41 FOREIGN KEY (BORROWAPPLYMASTERID)
      REFERENCES T_FB_BORROWAPPLYMASTER (BORROWAPPLYMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_BORROWAPPLYDETAIL ADD CONSTRAINT FK_REFERENCE_43 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_BORROWAPPLYMASTER ADD CONSTRAINT FK_REFERENCE_40 FOREIGN KEY (EXTENSIONALORDERID)
      REFERENCES T_FB_EXTENSIONALORDER (EXTENSIONALORDERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_BUDGETACCOUNT ADD CONSTRAINT FK_REFERENCE_26 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_BUDGETCHECK ADD CONSTRAINT FK_T_FB_BUD_REFERENCE_T_FB_SU2 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_CHARGEAPPLYDETAIL ADD CONSTRAINT FK_REFERENCE_49 FOREIGN KEY (CHARGEAPPLYMASTERID)
      REFERENCES T_FB_CHARGEAPPLYMASTER (CHARGEAPPLYMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_CHARGEAPPLYDETAIL ADD CONSTRAINT FK_T_FB_CHA_REFERENCE_T_FB_S41 FOREIGN KEY (BORROWAPPLYDETAILID)
      REFERENCES T_FB_BORROWAPPLYDETAIL (BORROWAPPLYDETAILID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_CHARGEAPPLYDETAIL ADD CONSTRAINT FK_REFERENCE_6 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_CHARGEAPPLYMASTER ADD CONSTRAINT FK_REFERENCE_38 FOREIGN KEY (EXTENSIONALORDERID)
      REFERENCES T_FB_EXTENSIONALORDER (EXTENSIONALORDERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_CHARGEAPPLYMASTER ADD CONSTRAINT FK_REFERENCE_44 FOREIGN KEY (BORROWAPPLYMASTERID)
      REFERENCES T_FB_BORROWAPPLYMASTER (BORROWAPPLYMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_CHARGEBYREPAY ADD CONSTRAINT FK_T_FB_CHA_REFERENCE_T_FB_CH1 FOREIGN KEY (CHARGEAPPLYMASTERID)
      REFERENCES T_FB_CHARGEAPPLYMASTER (CHARGEAPPLYMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_COMPANYBUDGETAPPLYDETAIL ADD CONSTRAINT FK_REFERENCE_10 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_COMPANYBUDGETAPPLYDETAIL ADD CONSTRAINT FK_T_FB_COM_REFERENCE_T_FB_CO1 FOREIGN KEY (COMPANYBUDGETAPPLYMASTERID)
      REFERENCES T_FB_COMPANYBUDGETAPPLYMASTER (COMPANYBUDGETAPPLYMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_COMPANYBUDGETMODDETAIL ADD CONSTRAINT FK_T_FB_COM_REFERENCE_T_FB_CO3 FOREIGN KEY (COMPANYBUDGETMODMASTERID)
      REFERENCES T_FB_COMPANYBUDGETMODMASTER (COMPANYBUDGETMODMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_COMPANYBUDGETMODDETAIL ADD CONSTRAINT FK_T_FB_COM_REFERENCE_T_FB_SU3 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_COMPANYBUDGETSUMDETAIL ADD CONSTRAINT FK_T_FB_COM_REFERENCE_T_FB_K02 FOREIGN KEY (COMPANYBUDGETAPPLYMASTERID)
      REFERENCES T_FB_COMPANYBUDGETAPPLYMASTER (COMPANYBUDGETAPPLYMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_COMPANYBUDGETSUMDETAIL ADD CONSTRAINT FK_T_FB_COM_REFERENCE_T_FB_K01 FOREIGN KEY (COMPANYBUDGETSUMMASTERID)
      REFERENCES T_FB_COMPANYBUDGETSUMMASTER (COMPANYBUDGETSUMMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_COMPANYTRANSFERDETAIL ADD CONSTRAINT FK_T_FB_COM_REFERENCE_T_FC_CA1 FOREIGN KEY (COMPANYTRANSFERMASTERID)
      REFERENCES T_FB_COMPANYTRANSFERMASTER (COMPANYTRANSFERMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_COMPANYTRANSFERDETAIL ADD CONSTRAINT FK_T_FB_TRA_REFERENCE_T_FB_SU1 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_DEPTBUDGETADDDETAIL ADD CONSTRAINT FK_REFERENCE_27 FOREIGN KEY (DEPTBUDGETADDMASTERID)
      REFERENCES T_FB_DEPTBUDGETADDMASTER (DEPTBUDGETADDMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_DEPTBUDGETADDDETAIL ADD CONSTRAINT FK_REFERENCE_28 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_DEPTBUDGETAPPLYDETAIL ADD CONSTRAINT FK_T_FB_DEP_REFERENCE_T_FB_DE2 FOREIGN KEY (DEPTBUDGETAPPLYMASTERID)
      REFERENCES T_FB_DEPTBUDGETAPPLYMASTER (DEPTBUDGETAPPLYMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_DEPTBUDGETAPPLYDETAIL ADD CONSTRAINT FK_T_FB_DEP_REFERENCE_T_FB_SU2 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_DEPTBUDGETSUMDETAIL ADD CONSTRAINT FK_T_FB_DEP_REFERENCE_T_FB_K04 FOREIGN KEY (DEPTBUDGETSUMMASTERID)
      REFERENCES T_FB_DEPTBUDGETSUMMASTER (DEPTBUDGETSUMMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_DEPTBUDGETSUMDETAIL ADD CONSTRAINT FK_T_FB_DEP_REFERENCE_T_FB_K03 FOREIGN KEY (DEPTBUDGETAPPLYMASTERID)
      REFERENCES T_FB_DEPTBUDGETAPPLYMASTER (DEPTBUDGETAPPLYMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_DEPTTRANSFERDETAIL ADD CONSTRAINT FK_T_FB_DEP_REFERENCE_T_FC_S11 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_DEPTTRANSFERDETAIL ADD CONSTRAINT FK_T_FB_DEP_REFERENCE_T_FC_222 FOREIGN KEY (DEPTTRANSFERMASTERID)
      REFERENCES T_FB_DEPTTRANSFERMASTER (DEPTTRANSFERMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_EXTENSIONORDERDETAIL ADD CONSTRAINT FK_REFERENCE_50 FOREIGN KEY (EXTENSIONALORDERID)
      REFERENCES T_FB_EXTENSIONALORDER (EXTENSIONALORDERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_EXTENSIONORDERDETAIL ADD CONSTRAINT FK_REFERENCE_51 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_EXTENSIONALORDER ADD CONSTRAINT FK_T_FB_EXT_REFERENCE_T_FB_S91 FOREIGN KEY (EXTENSIONALTYPEID)
      REFERENCES T_FB_EXTENSIONALTYPE (EXTENSIONALTYPEID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_PERSONBUDGETADDDETAIL ADD CONSTRAINT FK_T_FB_PER_REFERENCE_T_FB_D01 FOREIGN KEY (DEPTBUDGETADDDETAILID)
      REFERENCES T_FB_DEPTBUDGETADDDETAIL (DEPTBUDGETADDDETAILID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_PERSONBUDGETADDDETAIL ADD CONSTRAINT FK_T_FB_PER_REFERENCE_T_FB_S41 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_PERSONBUDGETAPPLYDETAIL ADD CONSTRAINT FK_T_FB_PER_REFERENCE_T_FB_D02 FOREIGN KEY (DEPTBUDGETAPPLYDETAILID)
      REFERENCES T_FB_DEPTBUDGETAPPLYDETAIL (DEPTBUDGETAPPLYDETAILID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_PERSONBUDGETAPPLYDETAIL ADD CONSTRAINT FK_REFERENCE_8 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_PERSONMONEYASSIGNDETAIL ADD CONSTRAINT FK_T_FB_PER_REFERENCE_T_FB_N01 FOREIGN KEY (PERSONMONEYASSIGNMASTERID)
      REFERENCES T_FB_PERSONMONEYASSIGNMASTER (PERSONMONEYASSIGNMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_PERSONMONEYASSIGNDETAIL ADD CONSTRAINT FK_T_FB_PER_REFERENCE_T_FB_N02 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_REPAYAPPLYDETAIL ADD CONSTRAINT FK_REFERENCE_42 FOREIGN KEY (REPAYAPPLYMASTERID)
      REFERENCES T_FB_REPAYAPPLYMASTER (REPAYAPPLYMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_REPAYAPPLYDETAIL ADD CONSTRAINT FK_REFERENCE_45 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_REPAYAPPLYDETAIL ADD CONSTRAINT FK_T_FB_REP_REFERENCE_T_FB_S43 FOREIGN KEY (BORROWAPPLYDETAILID)
      REFERENCES T_FB_BORROWAPPLYDETAIL (BORROWAPPLYDETAILID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_REPAYAPPLYMASTER ADD CONSTRAINT FK_T_FB_REP_REFERENCE_T_FB_S01 FOREIGN KEY (BORROWAPPLYMASTERID)
      REFERENCES T_FB_BORROWAPPLYMASTER (BORROWAPPLYMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_REPAYAPPLYMASTER ADD CONSTRAINT FK_REFERENCE_48 FOREIGN KEY (EXTENSIONALORDERID)
      REFERENCES T_FB_EXTENSIONALORDER (EXTENSIONALORDERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_SUBJECTCOMPANYSET ADD CONSTRAINT FK_REFERENCE_16 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_SUMSETTINGSDETAIL ADD CONSTRAINT FK_T_FB_SUMSETTINGSMASTER FOREIGN KEY (SUMSETTINGSMASTERID)
      REFERENCES T_FB_SUMSETTINGSMASTER (SUMSETTINGSMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_SUBJECT ADD CONSTRAINT FK_T_FB_SUB_REFERENCE_T_FB_SU4 FOREIGN KEY (SUBJECTTYPEID)
      REFERENCES T_FB_SUBJECTTYPE (SUBJECTTYPEID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_SUBJECT ADD CONSTRAINT FK_REFERENCE_9 FOREIGN KEY (PARENTSUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_SUBJECTCOMPANY ADD CONSTRAINT FK_T_FB_COM_REFERENCE_T_FB_SU0 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_SUBJECTDEPTMENT ADD CONSTRAINT FK_T_FB_SUB_REFERENCE_T_FB_S01 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_SUBJECTDEPTMENT ADD CONSTRAINT FK_T_FB_SUB_REFERENCE_T_FB_SU1 FOREIGN KEY (SUBJECTCOMPANYID)
      REFERENCES T_FB_SUBJECTCOMPANY (SUBJECTCOMPANYID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_SUBJECTPOST ADD CONSTRAINT FK_T_FB_SUB_REFERENCE_T_FB_SU2 FOREIGN KEY (SUBJECTDEPTMENTID)
      REFERENCES T_FB_SUBJECTDEPTMENT (SUBJECTDEPTMENTID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_SUBJECTPOST ADD CONSTRAINT FK_T_FB_SUB_REFERENCE_T_FB_S02 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_TRAVELEXPAPPLYDETAIL ADD CONSTRAINT FK_REFERENCE_2 FOREIGN KEY (TRAVELEXPAPPLYMASTERID)
      REFERENCES T_FB_TRAVELEXPAPPLYMASTER (TRAVELEXPAPPLYMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_TRAVELEXPAPPLYDETAIL ADD CONSTRAINT FK_REFERENCE_5 FOREIGN KEY (SUBJECTID)
      REFERENCES T_FB_SUBJECT (SUBJECTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_FB_TRAVELEXPAPPLYDETAIL ADD CONSTRAINT FK_T_FB_TRA_REFERENCE_T_FB_S42 FOREIGN KEY (BORROWAPPLYDETAILID)
      REFERENCES T_FB_BORROWAPPLYDETAIL (BORROWAPPLYDETAILID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_TRAVELEXPAPPLYMASTER ADD CONSTRAINT FK_REFERENCE_46 FOREIGN KEY (BORROWAPPLYMASTERID)
      REFERENCES T_FB_BORROWAPPLYMASTER (BORROWAPPLYMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_FB_TRAVELEXPAPPLYMASTER ADD CONSTRAINT FK_REFERENCE_47 FOREIGN KEY (EXTENSIONALORDERID)
      REFERENCES T_FB_EXTENSIONALORDER (EXTENSIONALORDERID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ADJUSTLEAVE ADD CONSTRAINT FK_REFERENCE_63 FOREIGN KEY (LEAVERECORDID)
      REFERENCES T_HR_EMPLOYEELEAVERECORD (LEAVERECORDID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_AREAALLOWANCE ADD CONSTRAINT FK_AREADIFFERENCE_T_HR_ARE FOREIGN KEY (AREADIFFERENCEID)
      REFERENCES T_HR_AREADIFFERENCE (AREADIFFERENCEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_AREACITY ADD CONSTRAINT FK_T_HR_ARE_AREACITY FOREIGN KEY (AREADIFFERENCEID)
      REFERENCES T_HR_AREADIFFERENCE (AREADIFFERENCEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ASSESSMENTFORMDETAIL ADD CONSTRAINT FK_T_HR_ASSEDETAIL_HR_ASS FOREIGN KEY (ASSESSMENTFORMMASTERID)
      REFERENCES T_HR_ASSESSMENTFORMMASTER (ASSESSMENTFORMMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ASSESSMENTFORMDETAIL ADD CONSTRAINT FK_REFERENCE_69 FOREIGN KEY (CHECKPOINTSETID)
      REFERENCES T_HR_CHECKPOINTSET (CHECKPOINTSETID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ASSESSMENTFORMMASTER ADD CONSTRAINT FK_T_HR_ASS_EMPLOYEECHECK FOREIGN KEY (BEREGULARID)
      REFERENCES T_HR_EMPLOYEECHECK (BEREGULARID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ASSESSMENTFORMMASTER ADD CONSTRAINT FK_T_HR_FORMMASTER_T_HR_EMP FOREIGN KEY (POSTCHANGEID)
      REFERENCES T_HR_EMPLOYEEPOSTCHANGE (POSTCHANGEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ATTENDFREELEAVE ADD CONSTRAINT FK_REFERENCE_78 FOREIGN KEY (ATTENDANCESOLUTIONID)
      REFERENCES T_HR_ATTENDANCESOLUTION (ATTENDANCESOLUTIONID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ATTENDFREELEAVE ADD CONSTRAINT FK_ATTENDFREELEAVE_T_HR_LEA FOREIGN KEY (LEAVETYPESETID)
      REFERENCES T_HR_LEAVETYPESET (LEAVETYPESETID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ATTENDMONTHLYBALANCE ADD CONSTRAINT FK_T_HR_ATT_BATCH_ATT FOREIGN KEY (MONTHLYBATCHID)
      REFERENCES T_HR_ATTENDMONTHLYBATCHBALANCE (MONTHLYBATCHID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ATTENDANCEDEDUCTDETAIL ADD CONSTRAINT FK_T_HR_ATT_T_HR_DEDUCTMASTER FOREIGN KEY (DEDUCTMASTERID)
      REFERENCES T_HR_ATTENDANCEDEDUCTMASTER (DEDUCTMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ATTENDANCERECORD ADD CONSTRAINT FK_REFERENCE_64 FOREIGN KEY (WORKTIMESETID)
      REFERENCES T_HR_SHIFTDEFINE (SHIFTDEFINEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ATTENDANCESOLUTION ADD CONSTRAINT FK_REFERENCE_65 FOREIGN KEY (OVERTIMEREWARDID)
      REFERENCES T_HR_OVERTIMEREWARD (OVERTIMEREWARDID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ATTENDANCESOLUTION ADD CONSTRAINT FK_REFERENCE_74 FOREIGN KEY (TEMPLATEMASTERID)
      REFERENCES T_HR_SCHEDULINGTEMPLATEMASTER (TEMPLATEMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ATTENDANCESOLUTIONASIGN ADD CONSTRAINT FK_T_HR_SOLUTIONAPL_T_HR_ATT FOREIGN KEY (ATTENDANCESOLUTIONID)
      REFERENCES T_HR_ATTENDANCESOLUTION (ATTENDANCESOLUTIONID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ATTENDANCESOLUTIONDEDUCT ADD CONSTRAINT FK_T_HR_SOLUTION_T_HR_ATT FOREIGN KEY (ATTENDANCESOLUTIONID)
      REFERENCES T_HR_ATTENDANCESOLUTION (ATTENDANCESOLUTIONID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ATTENDANCESOLUTIONDEDUCT ADD CONSTRAINT FK_T_HR_MASTER_T_HR_DETAIL FOREIGN KEY (DEDUCTMASTERID)
      REFERENCES T_HR_ATTENDANCEDEDUCTMASTER (DEDUCTMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_CHECKMODELDEFINE ADD CONSTRAINT FK_CHECKMODELFLOW_MODEL FOREIGN KEY (ORGCHECKMODELID)
      REFERENCES T_HR_ORGANIZATIONCHECKMODEL (ORGCHECKMODELID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_HR_CHECKPOINTLEVELSET ADD CONSTRAINT FK_T_HR_CHE_POINTLEVEL FOREIGN KEY (CHECKPOINTSETID)
      REFERENCES T_HR_CHECKPOINTSET (CHECKPOINTSETID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_CHECKPOINTSET ADD CONSTRAINT FK_REFERENCE_70 FOREIGN KEY (CHECKPROJECTID)
      REFERENCES T_HR_CHECKPROJECTSET (CHECKPROJECTID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_COMPANY ADD CONSTRAINT FK_T_HR_COM_T_HR_COM FOREIGN KEY (FATHERCOMPANYID)
      REFERENCES T_HR_COMPANY (COMPANYID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_COMPANYHISTORY ADD CONSTRAINT FK_T_HR_COMHIS_T_HR_COM FOREIGN KEY (FATHERCOMPANYID)
      REFERENCES T_HR_COMPANY (COMPANYID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_CUSTOMGUERDON ADD CONSTRAINT FK_T_HR_CUSTOMGUERDON_T_HR_SAL FOREIGN KEY (SALARYSTANDARDID)
      REFERENCES T_HR_SALARYSTANDARD (SALARYSTANDARDID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_CUSTOMGUERDON ADD CONSTRAINT FK_T_HR_CUS_GUERDONSET FOREIGN KEY (CUSTOMGUERDONSETID)
      REFERENCES T_HR_CUSTOMGUERDONSET (CUSTOMGUERDONSETID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_CUSTOMGUERDONARCHIVE ADD CONSTRAINT FK_T_HR_GUERDONARCHIVE_SAL FOREIGN KEY (SALARYARCHIVEID)
      REFERENCES T_HR_SALARYARCHIVE (SALARYARCHIVEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_CUSTOMGUERDONARCHIVEHIS ADD CONSTRAINT FK_T_HR_ARCHIVEHIS_T_HR_SAL FOREIGN KEY (SALARYARCHIVEID)
      REFERENCES T_HR_SALARYARCHIVEHIS (SALARYARCHIVEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_DEPARTMENT ADD CONSTRAINT FK_REFERENCE_139 FOREIGN KEY (COMPANYID)
      REFERENCES T_HR_COMPANY (COMPANYID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_DEPARTMENT ADD CONSTRAINT FK_T_HR_DEP_T_HR_DEPDIC FOREIGN KEY (DEPARTMENTDICTIONARYID)
      REFERENCES T_HR_DEPARTMENTDICTIONARY (DEPARTMENTDICTIONARYID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_DEPARTMENTHISTORY ADD CONSTRAINT FK_T_HR_DEPHIS_T_HR_DEPDIC FOREIGN KEY (DEPARTMENTDICTIONARYID)
      REFERENCES T_HR_DEPARTMENTDICTIONARY (DEPARTMENTDICTIONARYID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEE ADD CONSTRAINT FK_T_HR_EMPBASE_T_HR_RES FOREIGN KEY (RESUMEID)
      REFERENCES T_HR_RESUME (RESUMEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEEPOST ADD CONSTRAINT FK_REFERENCE_61 FOREIGN KEY (POSTID)
      REFERENCES T_HR_POST (POSTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEEPOST ADD CONSTRAINT FK_T_HR_EMPPOST_T_HR_EMP FOREIGN KEY (EMPLOYEEID)
      REFERENCES T_HR_EMPLOYEE (EMPLOYEEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EDUCATEHISTORY ADD CONSTRAINT FK_REFERENCE_104 FOREIGN KEY (RESUMEID)
      REFERENCES T_HR_RESUME (RESUMEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEEABNORMRECORD ADD CONSTRAINT FK_SIGNINDETAIL_T_HR_ATT FOREIGN KEY (ATTENDANCERECORDID)
      REFERENCES T_HR_ATTENDANCERECORD (ATTENDANCERECORDID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEEADDSUM ADD CONSTRAINT FK_EMPLOYEEADDSUMBATCH_EMP FOREIGN KEY (MONTHLYBATCHID)
      REFERENCES T_HR_EMPLOYEEADDSUMBATCH (MONTHLYBATCHID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEECANCELLEAVE ADD CONSTRAINT FK_REFERENCE_88 FOREIGN KEY (LEAVERECORDID)
      REFERENCES T_HR_EMPLOYEELEAVERECORD (LEAVERECORDID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEECHECK ADD CONSTRAINT FK_T_HR_EMPLOYEECHECK_T_HR_EMP FOREIGN KEY (EMPLOYEEID)
      REFERENCES T_HR_EMPLOYEE (EMPLOYEEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEECONTRACT ADD CONSTRAINT FK_T_HR_EMPCONTRACT_T_HR_EMP FOREIGN KEY (EMPLOYEEID)
      REFERENCES T_HR_EMPLOYEE (EMPLOYEEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEEENTRY ADD CONSTRAINT FK_T_HR_EMPENTRY_T_HR_EMPBASE FOREIGN KEY (EMPLOYEEID)
      REFERENCES T_HR_EMPLOYEE (EMPLOYEEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEEEVECTIONREPORT ADD CONSTRAINT FK_T_HR_EMP_EVECTIONREPORT FOREIGN KEY (EVECTIONRECORDID)
      REFERENCES T_HR_EMPLOYEEEVECTIONRECORD (EVECTIONRECORDID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEEINSURANCE ADD CONSTRAINT FK_T_HR_EMPSIRECORD_T_HR_EMP FOREIGN KEY (EMPLOYEEID)
      REFERENCES T_HR_EMPLOYEE (EMPLOYEEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEELEAVERECORD ADD CONSTRAINT FK_REFERENCE_71 FOREIGN KEY (LEAVETYPESETID)
      REFERENCES T_HR_LEAVETYPESET (LEAVETYPESETID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEELEVELDAYDETAILS ADD CONSTRAINT FK_LEVELDAYCOUNT_DETAILS FOREIGN KEY (RECORDID)
      REFERENCES T_HR_EMPLOYEELEVELDAYCOUNT (RECORDID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEEOVERTIMEDETAILRD ADD CONSTRAINT FK_T_HR_EMPORD_T_HR_EMPODRD FOREIGN KEY (OVERTIMERECORDID)
      REFERENCES T_HR_EMPLOYEEOVERTIMERECORD (OVERTIMERECORDID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEEPOSTCHANGE ADD CONSTRAINT FK_T_HR_EMPCHANGE_T_HR_EMPBASE FOREIGN KEY (EMPLOYEEID)
      REFERENCES T_HR_EMPLOYEE (EMPLOYEEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEESALARYRECORD ADD CONSTRAINT FK_SALARYRECORDBATCH_T_HR_SAL FOREIGN KEY (MONTHLYBATCHID)
      REFERENCES T_HR_SALARYRECORDBATCH (MONTHLYBATCHID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEESALARYRECORDITEM ADD CONSTRAINT FK_SALARYRECORDITEM_T_HR_EMP FOREIGN KEY (EMPLOYEESALARYRECORDID)
      REFERENCES T_HR_EMPLOYEESALARYRECORD (EMPLOYEESALARYRECORDID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEESIGNINDETAIL ADD CONSTRAINT FK_SIGNINDETAIL_SIGNINRECORD FOREIGN KEY (SIGNINID)
      REFERENCES T_HR_EMPLOYEESIGNINRECORD (SIGNINID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EMPLOYEESIGNINDETAIL ADD CONSTRAINT FK_SIGNINDETAIL_ABNORMRECORD FOREIGN KEY (ABNORMRECORDID)
      REFERENCES T_HR_EMPLOYEEABNORMRECORD (ABNORMRECORDID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_EXPERIENCE ADD CONSTRAINT FK_T_HR_EMPCAREER_T_HR_RES FOREIGN KEY (RESUMEID)
      REFERENCES T_HR_RESUME (RESUMEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_FAMILYMEMBER ADD CONSTRAINT FK_REFERENCE_110 FOREIGN KEY (EMPLOYEEID)
      REFERENCES T_HR_EMPLOYEE (EMPLOYEEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_FREELEAVEDAYSET ADD CONSTRAINT FK_REFERENCE_72 FOREIGN KEY (LEAVETYPESETID)
      REFERENCES T_HR_LEAVETYPESET (LEAVETYPESETID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_IMPORTSETDETAIL ADD CONSTRAINT FK_T_HR_IMPORTSETMASTER_DETAIL FOREIGN KEY (MASTERID)
      REFERENCES T_HR_IMPORTSETMASTER (MASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_KPIPOINT ADD CONSTRAINT FK_KPIPOINT_REFERENCE_T_HR_SCO FOREIGN KEY (SCORETYPEID)
      REFERENCES T_HR_SCORETYPE (SCORETYPEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_KPIPOINTDEFINE ADD CONSTRAINT KPIPOINTDEFINE_CHECKCATEGORY FOREIGN KEY (CHECKCATEGORYID)
      REFERENCES T_HR_CHECKCATEGORYSET (CHECKCATEGORYID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_HR_KPIPOINTDEFINE ADD CONSTRAINT FK_REFERENCE_96 FOREIGN KEY (SPOTCHECKGROUPID)
      REFERENCES T_HR_SPOTCHECKGROUP (SPOTCHECKGROUPID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_HR_KPIPOINTDEFINE ADD CONSTRAINT FK_KPIPOINTDEFINE_T_HR_CHE FOREIGN KEY (CHECKMODELID)
      REFERENCES T_HR_CHECKMODELDEFINE (CHECKMODELID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_HR_KPIREMIND ADD CONSTRAINT FK_T_HR_KPI_REFERENCE_T_HR_SCO FOREIGN KEY (SCORETYPEID)
      REFERENCES T_HR_SCORETYPE (SCORETYPEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_KPIRECORD ADD CONSTRAINT FK_T_HR_KPI_REFERENCE_T_HR_KPI FOREIGN KEY (KPIPOINTID)
      REFERENCES T_HR_KPIPOINT (KPIPOINTID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_KPIRECORD ADD CONSTRAINT FK_KPIRECORD_SPOTCHECKER FOREIGN KEY (T_H_SPOTCHECKERID)
      REFERENCES T_HR_SPOTCHECKER (SPOTCHECKERID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_HR_KPIRECORD ADD CONSTRAINT FK_T_HR_KPI_KPIPOINTDEFINE FOREIGN KEY (KPIPOINTID)
      REFERENCES T_HR_KPIPOINTDEFINE (KPIPOINTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_HR_KPIRECORDAPPEAL ADD CONSTRAINT FK_KPIRECORDAPPEAL_T_HR_KPI FOREIGN KEY (KPIRECORDID)
      REFERENCES T_HR_KPIRECORD (KPIRECORDID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_HR_KPITYPE ADD CONSTRAINT FK_KPITYPE_T_HR_SCORETYPE FOREIGN KEY (SCORETYPEID)
      REFERENCES T_HR_SCORETYPE (SCORETYPEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_LEFTOFFICE ADD CONSTRAINT FK_T_HR_EMPCHANGE_T_HR_EMP FOREIGN KEY (EMPLOYEEID)
      REFERENCES T_HR_EMPLOYEE (EMPLOYEEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_LEFTOFFICE ADD CONSTRAINT FK_REFERENCE_94 FOREIGN KEY (EMPLOYEEPOSTID)
      REFERENCES T_HR_EMPLOYEEPOST (EMPLOYEEPOSTID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_LEFTOFFICECONFIRM ADD CONSTRAINT FK_T_HR_LEF_LEFTOFFICECONFIRM FOREIGN KEY (DIMISSIONID)
      REFERENCES T_HR_LEFTOFFICE (DIMISSIONID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_ORGANIZATIONCHECKMODEL ADD CONSTRAINT FK_REFERENCE_97 FOREIGN KEY (FATHERLID)
      REFERENCES T_HR_ORGANIZATIONCHECKMODEL (ORGCHECKMODELID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_HR_OUTAPPLYCONFIRM ADD CONSTRAINT FK_T_HR_OUTAPPLY_OUTAPPLYCONFIRM FOREIGN KEY (OUTAPPLYID)
      REFERENCES T_HR_OUTAPPLYRECORD (OUTAPPLYID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_OUTPLANDAYS ADD CONSTRAINT FK_REFERENCE_93 FOREIGN KEY (VACATIONID)
      REFERENCES T_HR_VACATIONSET (VACATIONID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_PERFORMANCEDETAIL ADD CONSTRAINT FK_T_HR_PER_REFERENCE_T_HR_PER FOREIGN KEY (PERFORMANCEID)
      REFERENCES T_HR_PERFORMANCERECORD (PERFORMANCEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_PERFORMANCERECORD ADD CONSTRAINT FK_T_HR_PER_REFERENCE_T_HR_SUM FOREIGN KEY (SUMID)
      REFERENCES T_HR_SUMPERFORMANCERECORD (SUMID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_PENSIONDETAIL ADD CONSTRAINT FK_REFERENCE_109 FOREIGN KEY (PENSIONMASTERID)
      REFERENCES T_HR_PENSIONMASTER (PENSIONMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_PENSIONMASTER ADD CONSTRAINT FK_T_HR_EMPSI_T_HR_EMPBASE FOREIGN KEY (EMPLOYEEID)
      REFERENCES T_HR_EMPLOYEE (EMPLOYEEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_POST ADD CONSTRAINT FK_T_HR_POS_T_HR_POSDIC FOREIGN KEY (POSTDICTIONARYID)
      REFERENCES T_HR_POSTDICTIONARY (POSTDICTIONARYID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_HR_POST ADD CONSTRAINT FK_REFERENCE_144 FOREIGN KEY (DEPARTMENTID)
      REFERENCES T_HR_DEPARTMENT (DEPARTMENTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_HR_POSTDICTIONARY ADD CONSTRAINT FK_POSTDICTIONARY_T_HR_DEP FOREIGN KEY (DEPARTMENTDICTIONARYID)
      REFERENCES T_HR_DEPARTMENTDICTIONARY (DEPARTMENTDICTIONARYID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_POSTHISTORY ADD CONSTRAINT FK_T_HR_POSHIS_T_HR_POSDIC FOREIGN KEY (POSTDICTIONARYID)
      REFERENCES T_HR_POSTDICTIONARY (POSTDICTIONARYID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_POSTLEVELDISTINCTION ADD CONSTRAINT FK_REFERENCE_137 FOREIGN KEY (SALARYSYSTEMID)
      REFERENCES T_HR_SALARYSYSTEM (SALARYSYSTEMID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_RAMDONGROUPPERSON ADD CONSTRAINT FK_T_HR_RAM_REFERENCE_T_HR_RAN FOREIGN KEY (RANDOMGROUPID)
      REFERENCES T_HR_RANDOMGROUP (RANDOMGROUPID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_RELATIONPOST ADD CONSTRAINT FK_REFERENCE_146 FOREIGN KEY (POSTID)
      REFERENCES T_HR_POST (POSTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_HR_SCORETYPE ADD CONSTRAINT FK_T_HR_SCO_REFERENCE_T_HR_RAN FOREIGN KEY (RANDOMGROUPID)
      REFERENCES T_HR_RANDOMGROUP (RANDOMGROUPID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SALARYARCHIVE ADD CONSTRAINT FK_SALARYARCHIVE_T_HR_SAL FOREIGN KEY (SALARYSTANDARDID)
      REFERENCES T_HR_SALARYSTANDARD (SALARYSTANDARDID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SALARYARCHIVEHISITEM ADD CONSTRAINT FK_SALARYARCHIVEHIS_HISITEM FOREIGN KEY (SALARYARCHIVEID)
      REFERENCES T_HR_SALARYARCHIVEHIS (SALARYARCHIVEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SALARYARCHIVEITEM ADD CONSTRAINT FK_SALARYARCHIVEITEM_HR_SAL FOREIGN KEY (SALARYARCHIVEID)
      REFERENCES T_HR_SALARYARCHIVE (SALARYARCHIVEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SALARYLEVEL ADD CONSTRAINT FK_REFERENCE_128 FOREIGN KEY (POSTLEVELID)
      REFERENCES T_HR_POSTLEVELDISTINCTION (POSTLEVELID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SALARYSOLUTION ADD CONSTRAINT FK_SALARYSOLUTION_T_HR_ARE FOREIGN KEY (AREADIFFERENCEID)
      REFERENCES T_HR_AREADIFFERENCE (AREADIFFERENCEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SALARYSOLUTION ADD CONSTRAINT FK_SALARYSOLUTION_SALARYSYSTEM FOREIGN KEY (SALARYSYSTEMID)
      REFERENCES T_HR_SALARYSYSTEM (SALARYSYSTEMID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SALARYSOLUTIONASSIGN ADD CONSTRAINT FK_REFERENCE_127 FOREIGN KEY (SALARYSOLUTIONID)
      REFERENCES T_HR_SALARYSOLUTION (SALARYSOLUTIONID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SALARYSOLUTIONITEM ADD CONSTRAINT FK_SALARYSOLUTIONITEM_ITEM FOREIGN KEY (SALARYITEMID)
      REFERENCES T_HR_SALARYITEM (SALARYITEMID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SALARYSOLUTIONITEM ADD CONSTRAINT FK_SALARYSOLUTIONITEM_SOLUTION FOREIGN KEY (SALARYSOLUTIONID)
      REFERENCES T_HR_SALARYSOLUTION (SALARYSOLUTIONID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SALARYSTANDARD ADD CONSTRAINT FK_SALARYSTANDARD_SALARYLEVEL FOREIGN KEY (SALARYLEVELID)
      REFERENCES T_HR_SALARYLEVEL (SALARYLEVELID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SALARYSTANDARDITEM ADD CONSTRAINT FK_T_HR_SAL_SALARYSTANDARDITEM FOREIGN KEY (SALARYSTANDARDID)
      REFERENCES T_HR_SALARYSTANDARD (SALARYSTANDARDID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SALARYSTANDARDITEM ADD CONSTRAINT FK_SALARYSTANDARD_SALARYITEM FOREIGN KEY (SALARYITEMID)
      REFERENCES T_HR_SALARYITEM (SALARYITEMID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SALARYTAXES ADD CONSTRAINT FK_SALARYTAXES_SALARYSOLUTION FOREIGN KEY (SALARYSOLUTIONID)
      REFERENCES T_HR_SALARYSOLUTION (SALARYSOLUTIONID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SCHEDULINGTEMPLATEDETAIL ADD CONSTRAINT FK_REFERENCE_73 FOREIGN KEY (TEMPLATEMASTERID)
      REFERENCES T_HR_SCHEDULINGTEMPLATEMASTER (TEMPLATEMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SCHEDULINGTEMPLATEDETAIL ADD CONSTRAINT FK_REFERENCE_75 FOREIGN KEY (SHIFTDEFINEID)
      REFERENCES T_HR_SHIFTDEFINE (SHIFTDEFINEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_SPOTCHECKER ADD CONSTRAINT FK_REFERENCE_95 FOREIGN KEY (SPOTCHECKGROUPID)
      REFERENCES T_HR_SPOTCHECKGROUP (SPOTCHECKGROUPID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_HR_STANDREWARDARCHIVE ADD CONSTRAINT FK_T_HR_ARCHIVE_T_HR_SAL FOREIGN KEY (SALARYARCHIVEID)
      REFERENCES T_HR_SALARYARCHIVE (SALARYARCHIVEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_STANDREWARDARCHIVEHIS ADD CONSTRAINT FK_REWARDARCHIVE_T_HR_SAL FOREIGN KEY (SALARYARCHIVEID)
      REFERENCES T_HR_SALARYARCHIVEHIS (SALARYARCHIVEID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_STANDARDPERFORMANCEREWARD ADD CONSTRAINT FK_T_HR_STAREWARD_T_HR_SAL FOREIGN KEY (SALARYSTANDARDID)
      REFERENCES T_HR_SALARYSTANDARD (SALARYSTANDARDID) ON UPDATE RESTRICT;

ALTER TABLE T_HR_STANDARDPERFORMANCEREWARD ADD CONSTRAINT FK_REFERENCE_122 FOREIGN KEY (PERFORMANCEREWARDSETID)
      REFERENCES T_HR_PERFORMANCEREWARDSET (PERFORMANCEREWARDSETID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_ACCIDENTRECORD ADD CONSTRAINT FK_T_OA_ACCIDENTRECORD FOREIGN KEY (ASSETID)
      REFERENCES T_OA_VEHICLE (ASSETID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_AREAALLOWANCE ADD CONSTRAINT FK_T_OA_AREAALLOWANCE FOREIGN KEY (TRAVELSOLUTIONSID)
      REFERENCES T_OA_TRAVELSOLUTIONS (TRAVELSOLUTIONSID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_AREAALLOWANCE ADD CONSTRAINT FK_AREADIFFERENCE_T_OA_ARE FOREIGN KEY (AREADIFFERENCEID)
      REFERENCES T_OA_AREADIFFERENCE (AREADIFFERENCEID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_AREACITY ADD CONSTRAINT FK_T_OA_ARE_AREACITY FOREIGN KEY (AREADIFFERENCEID)
      REFERENCES T_OA_AREADIFFERENCE (AREADIFFERENCEID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_AREADIFFERENCE ADD CONSTRAINT FK_T_OA_AREADIFFERENCE FOREIGN KEY (TRAVELSOLUTIONSID)
      REFERENCES T_OA_TRAVELSOLUTIONS (TRAVELSOLUTIONSID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_BUSINESSREPORT ADD CONSTRAINT FK_REFERENCE_154 FOREIGN KEY (BUSINESSTRIPID)
      REFERENCES T_OA_BUSINESSTRIP (BUSINESSTRIPID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_OA_BUSINESSREPORTDETAIL ADD CONSTRAINT FK_T_OA_REPORTDETAIL_T_OA_BUS FOREIGN KEY (BUSINESSREPORTID)
      REFERENCES T_OA_BUSINESSREPORT (BUSINESSREPORTID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_BUSINESSTRIPDETAIL ADD CONSTRAINT FK_T_OA_BUS_T_OA_BUSDETAIL FOREIGN KEY (BUSINESSTRIPID)
      REFERENCES T_OA_BUSINESSTRIP (BUSINESSTRIPID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_CANTAKETHEPLANELINE ADD CONSTRAINT FK_REFERENCE_149 FOREIGN KEY (TRAVELSOLUTIONSID)
      REFERENCES T_OA_TRAVELSOLUTIONS (TRAVELSOLUTIONSID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_CONSERVATION ADD CONSTRAINT FK_T_OA_CONSERVATION FOREIGN KEY (ASSETID)
      REFERENCES T_OA_VEHICLE (ASSETID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_CONSERVATIONRECORD ADD CONSTRAINT FK_T_OA_CONSERVATIONRECORD FOREIGN KEY (CONSERVATIONID)
      REFERENCES T_OA_CONSERVATION (CONSERVATIONID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_CONTRACTPRINT ADD CONSTRAINT FK_T_OA_CONTRACTPRINT FOREIGN KEY (CONTRACTAPPID)
      REFERENCES T_OA_CONTRACTAPP (CONTRACTAPPID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_CONTRACTTEMPLATE ADD CONSTRAINT FK_T_OA_CONTRACTTEMPLATE FOREIGN KEY (CONTRACTTYPEID)
      REFERENCES T_OA_CONTRACTTYPE (CONTRACTTYPEID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_CONTRACTVIEW ADD CONSTRAINT FK_T_OA_CONTRACTVIEW FOREIGN KEY (CONTRACTPRINTID)
      REFERENCES T_OA_CONTRACTPRINT (CONTRACTPRINTID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_COSTRECORD ADD CONSTRAINT FK_T_OA_COSTRECORD FOREIGN KEY (ASSETID)
      REFERENCES T_OA_VEHICLE (ASSETID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_HIREAPP ADD CONSTRAINT FK_T_OA_HIREAPP FOREIGN KEY (HOUSELISTID)
      REFERENCES T_OA_HOUSELIST (HOUSELISTID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_HIRERECORD ADD CONSTRAINT T_OA_HIRERECORD FOREIGN KEY (HIREAPPID)
      REFERENCES T_OA_HIREAPP (HIREAPPID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_HOUSELIST ADD CONSTRAINT FK_T_OA_HOUSELIST1 FOREIGN KEY (HOUSEID)
      REFERENCES T_OA_HOUSEINFO (HOUSEID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_HOUSELIST ADD CONSTRAINT FK_T_OA_HOUSELIST2 FOREIGN KEY (ISSUANCEID)
      REFERENCES T_OA_HOUSEINFOISSUANCE (ISSUANCEID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_LENDARCHIVES ADD CONSTRAINT FK_T_OA_LENDARCHIVES FOREIGN KEY (ARCHIVESID)
      REFERENCES T_OA_ARCHIVES (ARCHIVESID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_LICENSEDETAIL ADD CONSTRAINT FK_T_OA_LICENSEDETAIL FOREIGN KEY (LICENSEMASTERID)
      REFERENCES T_OA_LICENSEMASTER (LICENSEMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_LICENSEMASTER ADD CONSTRAINT FK_T_OA_LICENSEMASTER FOREIGN KEY (ORGCODE)
      REFERENCES T_OA_ORGANIZATION (ORGCODE) ON UPDATE RESTRICT;

ALTER TABLE T_OA_LICENSEUSER ADD CONSTRAINT FK_T_OA_LICENSEUSER FOREIGN KEY (LICENSEMASTERID)
      REFERENCES T_OA_LICENSEMASTER (LICENSEMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_MAINTENANCEAPP ADD CONSTRAINT FK_T_OA_MAINTENANCEAPP FOREIGN KEY (ASSETID)
      REFERENCES T_OA_VEHICLE (ASSETID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_MAINTENANCERECORD ADD CONSTRAINT FK_T_OA_MAINTENANCERECORD FOREIGN KEY (MAINTENANCEAPPID)
      REFERENCES T_OA_MAINTENANCEAPP (MAINTENANCEAPPID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_MEETINGCONTENT ADD CONSTRAINT FK_T_OA_MEETINGCONTENT FOREIGN KEY (MEETINGINFOID, MEETINGUSERID)
      REFERENCES T_OA_MEETINGSTAFF (MEETINGINFOID, MEETINGUSERID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_MEETINGINFO ADD CONSTRAINT FK_T_OA_MEETINGINFO1 FOREIGN KEY (MEETINGROOMNAME)
      REFERENCES T_OA_MEETINGROOM (MEETINGROOMNAME) ON UPDATE RESTRICT;

ALTER TABLE T_OA_MEETINGINFO ADD CONSTRAINT FK_T_OA_MEETINGINFO2 FOREIGN KEY (MEETINGTYPE)
      REFERENCES T_OA_MEETINGTYPE (MEETINGTYPE) ON UPDATE RESTRICT;

ALTER TABLE T_OA_MEETINGMESSAGE ADD CONSTRAINT FK_T_OA_MEETINGMESSAGE FOREIGN KEY (MEETINGINFOID)
      REFERENCES T_OA_MEETINGINFO (MEETINGINFOID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_MEETINGROOMAPP ADD CONSTRAINT FK_T_OA_MEETINGROOMAPP FOREIGN KEY (MEETINGROOMNAME)
      REFERENCES T_OA_MEETINGROOM (MEETINGROOMNAME) ON UPDATE RESTRICT;

ALTER TABLE T_OA_MEETINGROOMTIMECHANGE ADD CONSTRAINT FK_T_OA_MEETINGROOMTIMECHANGE FOREIGN KEY (MEETINGROOMAPPID)
      REFERENCES T_OA_MEETINGROOMAPP (MEETINGROOMAPPID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_MEETINGSTAFF ADD CONSTRAINT FK_T_OA_MEETINGSTAFF FOREIGN KEY (MEETINGINFOID)
      REFERENCES T_OA_MEETINGINFO (MEETINGINFOID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_MEETINGTEMPLATE ADD CONSTRAINT FK_T_OA_MEETINGTEMPLATE FOREIGN KEY (MEETINGTYPE)
      REFERENCES T_OA_MEETINGTYPE (MEETINGTYPE) ON UPDATE RESTRICT;

ALTER TABLE T_OA_MEETINGTIMECHANGE ADD CONSTRAINT FK_T_OA_MEETINGTIMECHANGE FOREIGN KEY (MEETINGINFOID)
      REFERENCES T_OA_MEETINGINFO (MEETINGINFOID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_PROGRAMAPPLICATIONS ADD CONSTRAINT FK_REFERENCE_152 FOREIGN KEY (TRAVELSOLUTIONSID)
      REFERENCES T_OA_TRAVELSOLUTIONS (TRAVELSOLUTIONSID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_REIMBURSEMENTDETAIL ADD CONSTRAINT FK_REFERENCE_155 FOREIGN KEY (TRAVELREIMBURSEMENTID)
      REFERENCES T_OA_TRAVELREIMBURSEMENT (TRAVELREIMBURSEMENTID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_REQUIRE ADD CONSTRAINT FK_T_OA_REQUIRE FOREIGN KEY (REQUIREMASTERID)
      REFERENCES T_OA_REQUIREMASTER (REQUIREMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_REQUIREDETAIL ADD CONSTRAINT FK_T_OA_REQUIREDETAIL FOREIGN KEY (REQUIREMASTERID, SUBJECTID)
      REFERENCES T_OA_REQUIREDETAIL2 (REQUIREMASTERID, SUBJECTID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_REQUIREDETAIL2 ADD CONSTRAINT FK_T_OA_REQUIREDETAIL2 FOREIGN KEY (REQUIREMASTERID)
      REFERENCES T_OA_REQUIREMASTER (REQUIREMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_REQUIREDISTRIBUTE ADD CONSTRAINT FK_T_OA_REQUIREDISTRIBUTE FOREIGN KEY (REQUIREID)
      REFERENCES T_OA_REQUIRE (REQUIREID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_REQUIRERESULT ADD CONSTRAINT FK_T_OA_REQUIRERESULT FOREIGN KEY (REQUIREMASTERID)
      REFERENCES T_OA_REQUIREMASTER (REQUIREMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_REQUIRERESULT ADD CONSTRAINT FK_T_OA_REQUIRERESULT2 FOREIGN KEY (REQUIREID)
      REFERENCES T_OA_REQUIRE (REQUIREID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_SATISFACTIONDETAIL ADD CONSTRAINT FK_T_OA_SATISFACTIONDETAIL FOREIGN KEY (SATISFACTIONMASTERID)
      REFERENCES T_OA_SATISFACTIONMASTER (SATISFACTIONMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_SATISFACTIONDISTRIBUTE ADD CONSTRAINT FK_T_OA_SATISFACTIONDISTRIBUTE FOREIGN KEY (SATISFACTIONREQUIREID)
      REFERENCES T_OA_SATISFACTIONREQUIRE (SATISFACTIONREQUIREID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_SATISFACTIONREQUIRE ADD CONSTRAINT FK_T_OA_SATISFACTIONREQUIRE FOREIGN KEY (SATISFACTIONMASTERID)
      REFERENCES T_OA_SATISFACTIONMASTER (SATISFACTIONMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_SATISFACTIONRESULT ADD CONSTRAINT FK_T_OA_SATISFACTIONRESULT FOREIGN KEY (SATISFACTIONREQUIREID)
      REFERENCES T_OA_SATISFACTIONREQUIRE (SATISFACTIONREQUIREID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_SENDDOC ADD CONSTRAINT FK_T_OA_SENDDOC FOREIGN KEY (SENDDOCTYPE)
      REFERENCES T_OA_SENDDOCTYPE (SENDDOCTYPE) ON UPDATE RESTRICT;

ALTER TABLE T_OA_TAKETHESTANDARDTRANSPORT ADD CONSTRAINT FK_REFERENCE_148 FOREIGN KEY (TRAVELSOLUTIONSID)
      REFERENCES T_OA_TRAVELSOLUTIONS (TRAVELSOLUTIONSID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_TRAVELREIMBURSEMENT ADD CONSTRAINT FK_BUSINESSTRIP_T_OA_BUS FOREIGN KEY (BUSINESSTRIPID)
      REFERENCES T_OA_BUSINESSTRIP (BUSINESSTRIPID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_TRAVELREIMBURSEMENT ADD CONSTRAINT FK_REFERENCE_153 FOREIGN KEY (BUSINESSREPORTID)
      REFERENCES T_OA_BUSINESSREPORT (BUSINESSREPORTID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_OA_VEHICLECARD ADD CONSTRAINT FK_T_OA_VEHICLECARD FOREIGN KEY (ASSETID)
      REFERENCES T_OA_VEHICLE (ASSETID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_VEHICLEDISPATCH ADD CONSTRAINT FK_T_OA_VEHICLEDISPATCH FOREIGN KEY (ASSETID)
      REFERENCES T_OA_VEHICLE (ASSETID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_VEHICLEDISPATCHDETAIL ADD CONSTRAINT FK_T_OA_VEHICLEDISPATCHDETAIL1 FOREIGN KEY (VEHICLEDISPATCHID)
      REFERENCES T_OA_VEHICLEDISPATCH (VEHICLEDISPATCHID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_VEHICLEDISPATCHDETAIL ADD CONSTRAINT FK_T_OA_VEHICLEDISPATCHDETAIL2 FOREIGN KEY (VEHICLEUSEAPPID)
      REFERENCES T_OA_VEHICLEUSEAPP (VEHICLEUSEAPPID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_VEHICLEDISPATCHRECORD ADD CONSTRAINT FK_VEHICLEDISPATCHRECORD FOREIGN KEY (VEHICLEDISPATCHDETAILID)
      REFERENCES T_OA_VEHICLEDISPATCHDETAIL (VEHICLEDISPATCHDETAILID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_WELFAREDETAIL ADD CONSTRAINT FK_T_OA_WELFAREDETAIL FOREIGN KEY (WELFAREID)
      REFERENCES T_OA_WELFAREMASERT (WELFAREID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_WELFAREDISTRIBUTEDETAIL ADD CONSTRAINT FK_T_OA_WELFAREDISDETAIL FOREIGN KEY (WELFAREDISTRIBUTEMASTERID)
      REFERENCES T_OA_WELFAREDISTRIBUTEMASTER (WELFAREDISTRIBUTEMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_WELFAREDISTRIBUTEMASTER ADD CONSTRAINT FK_T_OA_WELFAREDISMASTER FOREIGN KEY (WELFAREID)
      REFERENCES T_OA_WELFAREMASERT (WELFAREID) ON UPDATE RESTRICT;

ALTER TABLE T_OA_WELFAREDISTRIBUTEUNDO ADD CONSTRAINT FK_T_OA_WELFAREDISTRIBUTEUNDO FOREIGN KEY (WELFAREDISTRIBUTEMASTERID)
      REFERENCES T_OA_WELFAREDISTRIBUTEMASTER (WELFAREDISTRIBUTEMASTERID) ON UPDATE RESTRICT;

ALTER TABLE T_SYS_DICTIONARY ADD CONSTRAINT FK_REFERENCE_161 FOREIGN KEY (FATHERID)
      REFERENCES T_SYS_DICTIONARY (DICTIONARYID) ON UPDATE RESTRICT;

ALTER TABLE T_SYS_ENTITYMENU ADD CONSTRAINT FK_REFERENCE_159 FOREIGN KEY (SUPERIORID)
      REFERENCES T_SYS_ENTITYMENU (ENTITYMENUID) ON UPDATE RESTRICT;

ALTER TABLE T_SYS_ENTITYMENUCUSTOMPERM ADD CONSTRAINT FK_REFERENCE_163 FOREIGN KEY (PERMISSIONID)
      REFERENCES T_SYS_PERMISSION (PERMISSIONID) ON UPDATE RESTRICT;

ALTER TABLE T_SYS_ENTITYMENUCUSTOMPERM ADD CONSTRAINT FK_T_SYS_RO_ENTITYMENU FOREIGN KEY (ENTITYMENUID)
      REFERENCES T_SYS_ENTITYMENU (ENTITYMENUID) ON UPDATE RESTRICT;

ALTER TABLE T_SYS_ENTITYMENUCUSTOMPERM ADD CONSTRAINT FK_REFERENCE_167 FOREIGN KEY (ROLEID)
      REFERENCES T_SYS_ROLE (ROLEID) ON UPDATE RESTRICT;

ALTER TABLE T_SYS_FBADMINROLE ADD CONSTRAINT FK_REFERENCE_169 FOREIGN KEY (FBADMINID)
      REFERENCES T_SYS_FBADMIN (FBADMINID) ON UPDATE RESTRICT;

ALTER TABLE T_SYS_PROVINCECITY ADD CONSTRAINT FK_T_SYS_PR_REFERENCE_T_SYS_PR FOREIGN KEY (FATHERID)
      REFERENCES T_SYS_PROVINCECITY (PROVINCEID) ON UPDATE RESTRICT;

ALTER TABLE T_SYS_PERMISSION ADD CONSTRAINT FK_REFERENCE_168 FOREIGN KEY (ENTITYMENUID)
      REFERENCES T_SYS_ENTITYMENU (ENTITYMENUID) ON UPDATE RESTRICT;

ALTER TABLE T_SYS_ROLEENTITYMENU ADD CONSTRAINT FK_T_SYS_RO_T_SYS_ROLE FOREIGN KEY (ROLEID)
      REFERENCES T_SYS_ROLE (ROLEID) ON UPDATE RESTRICT;

ALTER TABLE T_SYS_ROLEENTITYMENU ADD CONSTRAINT FK_T_ROLEENTITYMENU_T_SYS_EN FOREIGN KEY (ENTITYMENUID)
      REFERENCES T_SYS_ENTITYMENU (ENTITYMENUID) ON UPDATE RESTRICT;

ALTER TABLE T_SYS_ROLEMENUPERMISSION ADD CONSTRAINT FK_T_SYS_RO_ROLEENTITYMENU FOREIGN KEY (ROLEENTITYMENUID)
      REFERENCES T_SYS_ROLEENTITYMENU (ROLEENTITYMENUID) ON UPDATE RESTRICT;

ALTER TABLE T_SYS_ROLEMENUPERMISSION ADD CONSTRAINT FK_RELATIONSHIP_6 FOREIGN KEY (PERMISSIONID)
      REFERENCES T_SYS_PERMISSION (PERMISSIONID) ON UPDATE RESTRICT;

ALTER TABLE T_SYS_USERROLE ADD CONSTRAINT FK_REFERENCE_158 FOREIGN KEY (ROLEID)
      REFERENCES T_SYS_ROLE (ROLEID) ON UPDATE RESTRICT;

ALTER TABLE T_SYS_USERROLE ADD CONSTRAINT FK_REFERENCE_162 FOREIGN KEY (SYSUSERID)
      REFERENCES T_SYS_USER (SYSUSERID) ON UPDATE RESTRICT;

ALTER TABLE T_WF_DOTASKRULEDETAIL ADD CONSTRAINT FK_T_WF_DOT_REFERENCE_T_WF_DOT FOREIGN KEY (DOTASKRULEID)
      REFERENCES T_WF_DOTASKRULE (DOTASKRULEID) ON UPDATE RESTRICT;




/*==============================================================*/
/* Table: T_PF_ATTENTION                                        */
/*==============================================================*/
CREATE TABLE T_PF_ATTENTION
(
   ATTENTIONID          VARCHAR(50) NOT NULL,
   ORDERTYPE            VARCHAR(50),
   ORDERTYPENAME        VARCHAR(50),
   ORDERID              VARCHAR(50),
   ORDERTITLE           VARCHAR(200),
   ORDEROWNERID         VARCHAR(50),
   ORDEROWNERNAME       VARCHAR(50),
   ORDERURL             VARCHAR(500),
   ORDERSUBMITDATE      DATETIME,
   OWNERID              VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEUSERNAME       VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDATE           DATETIME ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (ATTENTIONID)
);

ALTER TABLE T_PF_ATTENTION COMMENT 'T_PF_ATTENTION';

/*==============================================================*/
/* Table: T_PF_DISTRIBUTEUSER                                   */
/*==============================================================*/
CREATE TABLE T_PF_DISTRIBUTEUSER
(
   DISTRIBUTEUSERID     VARCHAR(50) NOT NULL,
   MODELNAME            VARCHAR(50) NOT NULL,
   FORMID               VARCHAR(50) NOT NULL,
   VIEWTYPE             VARCHAR(1) NOT NULL DEFAULT '3' COMMENT '1：按公司，2：按部门，3：按用户',
   VIEWER               VARCHAR(50) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (DISTRIBUTEUSERID)
);

ALTER TABLE T_PF_DISTRIBUTEUSER COMMENT 'T_PF_DISTRIBUTEUSER';

/*==============================================================*/
/* Table: T_PF_FEEDBACK                                         */
/*==============================================================*/
CREATE TABLE T_PF_FEEDBACK
(
   FEEDID               VARCHAR(50) NOT NULL,
   ORDERTYPE            VARCHAR(50),
   ORDERTYPENAME        VARCHAR(50),
   ORDERID              VARCHAR(50),
   ORDERTITLE           VARCHAR(200),
   ORDEROWNERID         VARCHAR(50),
   ORDEROWNERNAME       VARCHAR(50),
   ORDERURL             VARCHAR(500),
   ORDERSUBMITDATE      DATETIME,
   OWNERID              VARCHAR(50),
   OWNERNAME            VARCHAR(50),
   OWNERCOMPANYID       VARCHAR(50),
   OWNERDEPARTMENTID    VARCHAR(50),
   OWNERPOSTID          VARCHAR(50),
   CREATEUSERID         VARCHAR(50),
   CREATEUSERNAME       VARCHAR(50),
   CREATECOMPANYID      VARCHAR(50),
   CREATEDEPARTMENTID   VARCHAR(50),
   CREATEPOSTID         VARCHAR(50),
   CREATEDATE           DATETIME ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   FEEDBACKCONTENT      VARCHAR(500) COMMENT '用户反馈意见',
   CUSTOMCONTENT        VARCHAR(500) COMMENT '客服反馈意见',
   CUSTOMDATE           DATETIME COMMENT '客服反馈时间',
   CUSTOMID             VARCHAR(50) COMMENT '客服ID',
   CUSTOMNAME           VARCHAR(50) COMMENT '客服姓名',
   FEEDTYPE             VARCHAR(1) DEFAULT '0' COMMENT '反馈类型 0 PC版反馈 1 手机反馈',
   PRIMARY KEY (FEEDID)
);

ALTER TABLE T_PF_FEEDBACK COMMENT '客户反馈';

/*==============================================================*/
/* Table: T_PF_NEWS                                             */
/*==============================================================*/
CREATE TABLE T_PF_NEWS
(
   NEWSID               VARCHAR(50) NOT NULL,
   NEWSTYPEID           VARCHAR(50) NOT NULL,
   NEWSTITEL            VARCHAR(200) NOT NULL,
   NEWSCONTENT          LONGBLOB NOT NULL,
   READCOUNT            VARCHAR(50),
   COMMENTCOUNT         VARCHAR(50),
   NEWSSTATE            VARCHAR(50) NOT NULL DEFAULT '0' COMMENT '0:未发布，1:已发布,2:已关闭',
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   ISIMAGE              VARCHAR(50) DEFAULT '0' COMMENT '图片',
   ISPOPUP              VARCHAR(1) DEFAULT '0' COMMENT '0:不弹出 1：弹出新闻',
   ENDDATE              DATETIME,
   PUTDEPTNAME          VARCHAR(50),
   PUTDEPTID            VARCHAR(50),
   ISPARTY              VARCHAR(1) DEFAULT '0' COMMENT '是否党办新闻:0:否 1:是',
   PRIMARY KEY (NEWSID)
);

ALTER TABLE T_PF_NEWS COMMENT '包含新闻、动态、通知、公告信息发布与管理';

/*==============================================================*/
/* Table: T_PF_NEWSCOMMENT                                      */
/*==============================================================*/
CREATE TABLE T_PF_NEWSCOMMENT
(
   COMMENTID            VARCHAR(50) NOT NULL,
   NEWSID               VARCHAR(50),
   COMMENTCONTENT       VARCHAR(500),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (COMMENTID)
);

ALTER TABLE T_PF_NEWSCOMMENT COMMENT '新闻评论表';

/*==============================================================*/
/* Table: T_PF_NEWSTYPE                                         */
/*==============================================================*/
CREATE TABLE T_PF_NEWSTYPE
(
   NEWSTYPEID           VARCHAR(50) NOT NULL,
   NEWSTYPENAME         VARCHAR(50),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (NEWSTYPEID)
);

ALTER TABLE T_PF_NEWSTYPE COMMENT '新闻类型';

/*==============================================================*/
/* Table: T_PF_PERSONALRECORD                                   */
/*==============================================================*/
CREATE TABLE T_PF_PERSONALRECORD
(
   PERSONALRECORDID     VARCHAR(50) NOT NULL COMMENT '个人单据ID',
   SYSTYPE              VARCHAR(50) COMMENT '系统类型',
   MODELCODE            VARCHAR(50) COMMENT '所属模块代码',
   MODELID              VARCHAR(50) COMMENT '单据ID',
   CHECKSTATE           VARCHAR(50) COMMENT '单据审核状态',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   CREATEDATE           DATETIME  COMMENT '创建时间',
   UPDATEDATE           DATETIME  COMMENT '提交时间',
   CONFIGINFO           VARCHAR(2000) COMMENT '参数配置',
   MODELDESCRIPTION     VARCHAR(2000) COMMENT '单据简要描叙',
   ISFORWARD            VARCHAR(10) COMMENT '是否转发("0"表示非转发，"1"表示转发)',
   ISVIEW               VARCHAR(10) COMMENT '是否已查看("0"表示未查看，"1"表示已查看)',
   PRIMARY KEY (PERSONALRECORDID)
);

ALTER TABLE T_PF_PERSONALRECORD COMMENT '个人单据';

/*==============================================================*/
/* Table: T_PF_PLATFORMCONFIG                                   */
/*==============================================================*/
CREATE TABLE T_PF_PLATFORMCONFIG
(
   CONFIGID             VARCHAR(50) NOT NULL,
   PARENTID             VARCHAR(50),
   PIOCPATH             VARCHAR(200),
   ISRESIZABLE          VARCHAR(1) NOT NULL DEFAULT '1' COMMENT '0:不可以，1：可以',
   TITEL                VARCHAR(200) NOT NULL,
   FULLNAME             VARCHAR(200) NOT NULL,
   ASSEMPLYNAME         VARCHAR(200) NOT NULL,
   PROGRAMTYPE          VARCHAR(200),
   ISSYSSHORTCUT        VARCHAR(1) DEFAULT '0' COMMENT '0：不是，1：是',
   ISSYSNEED            VARCHAR(1) DEFAULT '0' COMMENT '0:不可以，1：可以',
   INITPARAMS           VARCHAR(500),
   ISWEBPART            VARCHAR(1) DEFAULT '1' COMMENT '0：不是，1：是',
   USERSTATE            VARCHAR(20) NOT NULL,
   SYSTEMCODE           VARCHAR(20) NOT NULL,
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   ISDEFAULT            VARCHAR(1) DEFAULT '0' COMMENT '0：不使用，1：使用',
   PRIMARY KEY (CONFIGID)
);

ALTER TABLE T_PF_PLATFORMCONFIG COMMENT 'T_PF_PLATFORMCONFIG';

/*==============================================================*/
/* Table: T_PF_PROJECTCONFIG                                    */
/*==============================================================*/
CREATE TABLE T_PF_PROJECTCONFIG
(
   PROJECTID            VARCHAR(50) NOT NULL,
   SYSTEMCODE           VARCHAR(50) NOT NULL,
   PROJECTNAME          VARCHAR(200) NOT NULL,
   VERSIONFILENAME      VARCHAR(500) NOT NULL,
   DESCRIPTION          VARCHAR(2000),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (PROJECTID)
);

ALTER TABLE T_PF_PROJECTCONFIG COMMENT '平台子项目配置表。仅有超级管理员有此系统权限';

/*==============================================================*/
/* Table: T_PF_READRECORD                                       */
/*==============================================================*/
CREATE TABLE T_PF_READRECORD
(
   READRECORDID         VARCHAR(50) NOT NULL,
   EMPLOYEEID           VARCHAR(50),
   ORDERID              VARCHAR(50),
   ORDERTYPE            NUMERIC(38,0),
   PRIMARY KEY (READRECORDID)
);

ALTER TABLE T_PF_READRECORD COMMENT 'T_PF_READRECORD';

/*==============================================================*/
/* Index: INDEX_PF_READRECORD                                   */
/*==============================================================*/
CREATE UNIQUE INDEX INDEX_PF_READRECORD ON T_PF_READRECORD
(
   EMPLOYEEID,
   ORDERTYPE,
   ORDERID
);

/*==============================================================*/
/* Table: T_PF_USERCONFIG                                       */
/*==============================================================*/
CREATE TABLE T_PF_USERCONFIG
(
   USERCONFIGID         VARCHAR(50) NOT NULL,
   USERID               VARCHAR(50),
   CONFIGNAME           VARCHAR(50),
   CONFIGINFO           VARCHAR(2000),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (USERCONFIGID)
);

ALTER TABLE T_PF_USERCONFIG COMMENT 'T_PF_USERCONFIG';

/*==============================================================*/
/* Table: T_PF_USERCONFIGRELEVANCE                              */
/*==============================================================*/
CREATE TABLE T_PF_USERCONFIGRELEVANCE
(
   USERCONFIGRELEVANCEID VARCHAR(50) NOT NULL,
   USERID               VARCHAR(50) NOT NULL,
   CONFIGID             VARCHAR(50),
   TEMPLATENAME         VARCHAR(200) NOT NULL,
   CUSTOMPARAMS         VARCHAR(500),
   OWNERID              VARCHAR(50) NOT NULL,
   OWNERNAME            VARCHAR(50) NOT NULL,
   OWNERCOMPANYID       VARCHAR(50) NOT NULL,
   OWNERDEPARTMENTID    VARCHAR(50) NOT NULL,
   OWNERPOSTID          VARCHAR(50) NOT NULL,
   CREATEUSERID         VARCHAR(50) NOT NULL,
   CREATEUSERNAME       VARCHAR(50) NOT NULL,
   CREATECOMPANYID      VARCHAR(50) NOT NULL,
   CREATEDEPARTMENTID   VARCHAR(50) NOT NULL,
   CREATEPOSTID         VARCHAR(50) NOT NULL,
   CREATEDATE           DATETIME NOT NULL ,
   UPDATEUSERID         VARCHAR(50),
   UPDATEUSERNAME       VARCHAR(50),
   UPDATEDATE           DATETIME,
   PRIMARY KEY (USERCONFIGRELEVANCEID)
);

ALTER TABLE T_PF_USERCONFIGRELEVANCE COMMENT 'T_PF_USERCONFIGRELEVANCE';

ALTER TABLE T_PF_USERCONFIGRELEVANCE ADD CONSTRAINT FK_T_PF_USERCONFIGRELEVANCE FOREIGN KEY (CONFIGID)
      REFERENCES T_PF_PLATFORMCONFIG (CONFIGID) ON UPDATE RESTRICT;

alter table T_HR_EMPLOYEECHANGEHISTORY
  add constraint FK_T_HR_EMP_REFERENCE_T_HR_PC foreign key (EMPLOYEEID)
  references T_HR_EMPLOYEE (EMPLOYEEID)  ON UPDATE RESTRICT;

