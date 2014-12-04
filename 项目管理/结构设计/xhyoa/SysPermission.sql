/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2013/9/18 17:24:11                           */
/*==============================================================*/


DROP TABLE IF EXISTS T_SYS_DICTIONARY;

DROP TABLE IF EXISTS T_SYS_ENTITYMENU;

DROP TABLE IF EXISTS T_SYS_PERMISSION;

DROP TABLE IF EXISTS T_SYS_ROLE;

DROP TABLE IF EXISTS T_SYS_ROLEMENUPERMISSION;

DROP TABLE IF EXISTS T_SYS_ROLEMENUPERMISSIONRANGE;

DROP TABLE IF EXISTS T_SYS_USER;

DROP TABLE IF EXISTS T_SYS_USERACTLOG;

DROP TABLE IF EXISTS T_SYS_USERDATALOG;

DROP TABLE IF EXISTS T_SYS_USERLOGINRECORD;

DROP TABLE IF EXISTS T_SYS_USERROLE;

/*==============================================================*/
/* Table: T_SYS_DICTIONARY                                      */
/*==============================================================*/
CREATE TABLE T_SYS_DICTIONARY
(
   DICTIONARYID         VARCHAR(50) NOT NULL COMMENT '字典ID',
   FATHERID             VARCHAR(50) COMMENT '父字典ID',
   SYSTEMNAME           VARCHAR(100) COMMENT '系统类型名',
   SYSTEMCODE           VARCHAR(100) COMMENT '系统类型编码',
   DICTIONCATEGORYNAME  VARCHAR(100) COMMENT '字典类型名',
   DICTIONCATEGORY      VARCHAR(100) COMMENT '字典类型编码',
   DICTIONARYNAME       VARCHAR(100) COMMENT '字典名称',
   DICTIONARYVALUE      VARCHAR(100) COMMENT '字典值',
   ORDERNUMBER          NUMERIC(8,0) COMMENT '排序号',
   SYSTEMNEED           VARCHAR(1) COMMENT '系统必须字典:用户不能修改' COMMENT '系统必须字典:用户不能修改',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (DICTIONARYID)
);

ALTER TABLE T_SYS_DICTIONARY COMMENT '系统数据字典';

/*==============================================================*/
/* Table: T_SYS_ENTITYMENU                                      */
/*==============================================================*/
CREATE TABLE T_SYS_ENTITYMENU
(
   ENTITYMENUID         VARCHAR(50) NOT NULL COMMENT '实体菜单ID',
   SYSTEMTYPE           VARCHAR(50) NOT NULL COMMENT '系统类型',
   CHILDSYSTEMNAME      VARCHAR(500) COMMENT '子系统名称',
   ENTITYNAME           VARCHAR(50) COMMENT '实体名称',
   ENTITYCODE           VARCHAR(50) COMMENT '实体编码',
   HASSYSTEMMENU        VARCHAR(1) COMMENT '是否有系统菜单',
   SUPERIORID           VARCHAR(50) COMMENT '父级菜单ID',
   MENUCODE             VARCHAR(50) NOT NULL COMMENT '菜单编号',
   ORDERNUMBER          INT NOT NULL COMMENT '菜单序号',
   MENUNAME             VARCHAR(50) NOT NULL COMMENT '菜单名称',
   MENUICONPATH         VARCHAR(100) COMMENT '菜单图标地址',
   URLADDRESS           VARCHAR(500) COMMENT '链接地址',
   REMARK               VARCHAR(2000) COMMENT '备注',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (ENTITYMENUID)
);

ALTER TABLE T_SYS_ENTITYMENU COMMENT '系统实体菜单';

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
   UPDATEDATE           DATETIME COMMENT '修改时间',
   ENTITYMENUID         VARCHAR(50) COMMENT '实体菜单ID',
   ISCOMMOM             VARCHAR(50) COMMENT '是否公共权限项',
   PERMISSIONCODE       VARCHAR(200) COMMENT '权限编码',
   PRIMARY KEY (PERMISSIONID)
);

ALTER TABLE T_SYS_PERMISSION COMMENT '操作权限
增,删,改,查,导入,导出,报表,打印,共享数据....';

/*==============================================================*/
/* Table: T_SYS_ROLE                                            */
/*==============================================================*/
CREATE TABLE T_SYS_ROLE
(
   ROLEID               VARCHAR(50) NOT NULL COMMENT '角色ID',
   ROLENAME             VARCHAR(50) COMMENT '角色名称',
   SYSTEMTYPE           VARCHAR(1) COMMENT '系统类型',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (ROLEID)
);

ALTER TABLE T_SYS_ROLE COMMENT '角色';

/*==============================================================*/
/* Table: T_SYS_ROLEMENUPERMISSION                              */
/*==============================================================*/
CREATE TABLE T_SYS_ROLEMENUPERMISSION
(
   ROLEMENUPERMID       VARCHAR(50) NOT NULL COMMENT '角色菜单权限ID',
   ROLEID               VARCHAR(50) COMMENT '角色ID',
   ENTITYMENUID         VARCHAR(50) COMMENT '实体菜单ID',
   PERMISSIONID         VARCHAR(50) COMMENT '权限ID',
   DATARANGE            VARCHAR(50) COMMENT '数据范围',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   EXTENDVALUE          VARCHAR(50) COMMENT '扩展值',
   ISCUSTOMERDATARANGE  VARCHAR(32) COMMENT '是否指定范围',
   PRIMARY KEY (ROLEMENUPERMID)
);

ALTER TABLE T_SYS_ROLEMENUPERMISSION COMMENT '角色菜单权限';

/*==============================================================*/
/* Table: T_SYS_ROLEMENUPERMISSIONRANGE                         */
/*==============================================================*/
CREATE TABLE T_SYS_ROLEMENUPERMISSIONRANGE
(
   ENTITYMENUCUSTOMPERMID VARCHAR(50) NOT NULL COMMENT '自定义菜单权限ID',
   ROLEMENUPERMID       VARCHAR(50) COMMENT '角色菜单权限ID',
   ORGTYPE              VARCHAR(32) COMMENT '0 个人;1 岗位;2 部门;3公司' COMMENT '0 个人;1 岗位;2 部门;3公司',
   ORGID                VARCHAR(32) COMMENT '授权组织id',
   ORGNAME              VARCHAR(32) COMMENT '授权组织名称',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   STARTDATE            DATETIME COMMENT '开始时间',
   ENDDATE              DATETIME COMMENT '失效时间',
   REMARK               VARCHAR(2000) COMMENT '备注',
   PRIMARY KEY (ENTITYMENUCUSTOMPERMID)
);

ALTER TABLE T_SYS_ROLEMENUPERMISSIONRANGE COMMENT '角色与需要权限控制的实体的权限关系
角色对每个实体的增删改查的权限
角色对实体的权限范围：是对岗';

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
   ISMANGER             INT COMMENT '是否为管理员',
   STATE                VARCHAR(50) COMMENT '状态',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
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
   PERMISSIONID         VARCHAR(32) COMMENT '操作权限',
   LOGTYPE              VARCHAR(2) COMMENT '0,行为日志
            1,数据日志' COMMENT '0,行为日志
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
            1,数据日志' COMMENT '0,行为日志
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

ALTER TABLE T_SYS_USERDATALOG COMMENT '用户数据反馈日志表';

/*==============================================================*/
/* Table: T_SYS_USERLOGINRECORD                                 */
/*==============================================================*/
CREATE TABLE T_SYS_USERLOGINRECORD
(
   LOGINRECORDID        VARCHAR(50) NOT NULL COMMENT '登录系统记录ID',
   USERNAME             VARCHAR(50) NOT NULL COMMENT '员工系统帐号',
   EMPLOYEENAME         VARCHAR(200) COMMENT '员工名称',
   LOGINYEAR            INT COMMENT '登录年份',
   LOGINMONTH           INT COMMENT '登录月份',
   LOGINDATE            DATETIME COMMENT '登录日期',
   LOGINTIME            VARCHAR(50) COMMENT '登录时间',
   LOGINIP              VARCHAR(50) COMMENT '登录IP',
   REMARK               VARCHAR(2000) COMMENT '备注',
   OWNERCOMPANYID       VARCHAR(50) COMMENT '所属公司ID',
   OWNERID              VARCHAR(50) COMMENT '所属员工ID',
   OWNERPOSTID          VARCHAR(50) COMMENT '所属岗位ID',
   OWNERDEPARTMENTID    VARCHAR(50) COMMENT '所属部门ID',
   COMPANYNAME          VARCHAR(200) COMMENT '公司名称',
   DEPARTMENTNAME       VARCHAR(200) COMMENT '部门名称',
   POSTNAME             VARCHAR(200) COMMENT '岗位名称',
   PRIMARY KEY (LOGINRECORDID)
);

ALTER TABLE T_SYS_USERLOGINRECORD COMMENT '用户登录系统记录表';

/*==============================================================*/
/* Table: T_SYS_USERROLE                                        */
/*==============================================================*/
CREATE TABLE T_SYS_USERROLE
(
   USERROLEID           VARCHAR(50) NOT NULL COMMENT '用户角色ID',
   ROLEID               VARCHAR(50) COMMENT '角色ID',
   OWNERCOMPANYID       VARCHAR(50) NOT NULL COMMENT '所属公司ID',
   POSTID               VARCHAR(50) NOT NULL COMMENT '岗位ID',
   EMPLOYEEPOSTID       VARCHAR(50) NOT NULL COMMENT '员工岗位id',
   SYSUSERID            VARCHAR(50) COMMENT '系统用户ID',
   CREATEUSER           VARCHAR(50) COMMENT '创建人',
   CREATEDATE           DATETIME COMMENT '创建时间',
   UPDATEUSER           VARCHAR(50) COMMENT '修改人',
   UPDATEDATE           DATETIME COMMENT '修改时间',
   PRIMARY KEY (USERROLEID)
);

ALTER TABLE T_SYS_USERROLE COMMENT '用户角色';

ALTER TABLE T_SYS_DICTIONARY ADD CONSTRAINT FK_REFERENCE_14 FOREIGN KEY (FATHERID)
      REFERENCES T_SYS_DICTIONARY (DICTIONARYID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_SYS_ENTITYMENU ADD CONSTRAINT FK_REFERENCE_10 FOREIGN KEY (SUPERIORID)
      REFERENCES T_SYS_ENTITYMENU (ENTITYMENUID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_SYS_PERMISSION ADD CONSTRAINT FK_REFERENCE_19 FOREIGN KEY (ENTITYMENUID)
      REFERENCES T_SYS_ENTITYMENU (ENTITYMENUID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_SYS_ROLEMENUPERMISSION ADD CONSTRAINT FK_REFERENCE_11 FOREIGN KEY (ROLEID)
      REFERENCES T_SYS_ROLE (ROLEID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_SYS_ROLEMENUPERMISSION ADD CONSTRAINT FK_REFERENCE_15 FOREIGN KEY (ENTITYMENUID)
      REFERENCES T_SYS_ENTITYMENU (ENTITYMENUID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_SYS_ROLEMENUPERMISSION ADD CONSTRAINT FK_RELATIONSHIP_6 FOREIGN KEY (PERMISSIONID)
      REFERENCES T_SYS_PERMISSION (PERMISSIONID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_SYS_ROLEMENUPERMISSIONRANGE ADD CONSTRAINT FK_REFERENCE_13 FOREIGN KEY (ROLEMENUPERMID)
      REFERENCES T_SYS_ROLEMENUPERMISSION (ROLEMENUPERMID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_SYS_USERROLE ADD CONSTRAINT FK_REFERENCE_12 FOREIGN KEY (SYSUSERID)
      REFERENCES T_SYS_USER (SYSUSERID) ON DELETE RESTRICT ON UPDATE RESTRICT;

ALTER TABLE T_SYS_USERROLE ADD CONSTRAINT FK_REFERENCE_7 FOREIGN KEY (ROLEID)
      REFERENCES T_SYS_ROLE (ROLEID) ON DELETE RESTRICT ON UPDATE RESTRICT;

