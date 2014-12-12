------------------------------------------
-- Export file for user SMTOA           --
-- Created by LJX on 2010-6-25, 8:54:36 --
------------------------------------------

spool 111.log

prompt
prompt Creating table T_OA_HIREAPP
prompt ===========================
prompt
create table T_OA_HIREAPP
(
  HIREAPPID          NVARCHAR2(50) not null,
  HOUSELISTID        NVARCHAR2(50) not null,
  RENTCOST           INTEGER default 0 not null,
  DEPOSIT            INTEGER default 0 not null,
  MANAGECOST         INTEGER default 0 not null,
  STARTDATE          DATE not null,
  ENDDATE            DATE not null,
  BACKDATE           DATE,
  RENTTYPE           NVARCHAR2(1) default '0' not null,
  SETTLEMENTTYPE     NVARCHAR2(1) default '0' not null,
  ISBACK             NVARCHAR2(1) default '0' not null,
  ISOK               NVARCHAR2(1) default '0' not null,
  CHECKSTATE         NVARCHAR2(1) default '0' not null,
  OWNERID            NVARCHAR2(50) not null,
  OWNERNAME          NVARCHAR2(50) not null,
  OWNERCOMPANYID     NVARCHAR2(50) not null,
  OWNERDEPARTMENTID  NVARCHAR2(50) not null,
  OWNERPOSTID        NVARCHAR2(50) not null,
  CREATEUSERID       NVARCHAR2(50) not null,
  CREATEUSERNAME     NVARCHAR2(50) not null,
  CREATECOMPANYID    NVARCHAR2(50) not null,
  CREATEDEPARTMENTID NVARCHAR2(50) not null,
  CREATEPOSTID       NVARCHAR2(50) not null,
  CREATEDATE         DATE default SYSDATE not null,
  UPDATEUSERID       NVARCHAR2(50),
  UPDATEUSERNAME     NVARCHAR2(50),
  UPDATEDATE         DATE
)
;
comment on column T_OA_HIREAPP.ENDDATE
  is '预计到期时间，实际到期以退房时间为准';
comment on column T_OA_HIREAPP.BACKDATE
  is '退房时修改';
comment on column T_OA_HIREAPP.RENTTYPE
  is '0：合租；1：整租；';
comment on column T_OA_HIREAPP.SETTLEMENTTYPE
  is '0：工资扣；1：现金；';
comment on column T_OA_HIREAPP.ISBACK
  is '0:未退，1：已退';
comment on column T_OA_HIREAPP.ISOK
  is '0:未确认，1：已退认，2：取消';
comment on column T_OA_HIREAPP.CHECKSTATE
  is '0：未提交；1：审核中；2：审核通过；3：审核未通过';
alter table T_OA_HIREAPP
  add constraint PK_T_OA_HIREAPP primary key (HIREAPPID);
alter table T_OA_HIREAPP
  add constraint FK_T_OA_HIREAPP foreign key (HOUSELISTID)
  references T_OA_HOUSELIST (HOUSELISTID);


spool off












错误	59	“SMT.SaaS.OA.UI.SmtOAPersonOfficeService.T_OA_REQUIREMASTER”不包含“REQUIREID”的定义，并且找不到可接受类型为“SMT.SaaS.OA.UI.SmtOAPersonOfficeService.T_OA_REQUIREMASTER”的第一个参数的扩展方法“REQUIREID”(是否缺少 using 指令或程序集引用?)	D:\smt\SMT.SaaS.OA\SMT.SaaS.OA.UI\Views\EmployeeSatisfactionSurveys\EmployeeSubmissionsForm.xaml.cs	138	67	SMT.SaaS.OA.UI


错误	80	找不到类型或命名空间名称“EmployeeSubmissions”(是否缺少 using 指令或程序集引用?)	D:\smt\SMT.SaaS.OA\SMT.SaaS.OA.UI\Views\EmployeeSatisfactionSurveys\FrmEmployeeSurveysResult.xaml.cs	271	21	SMT.SaaS.OA.UI

