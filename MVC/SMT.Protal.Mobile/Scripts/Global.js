function global()
{ }
global.prototype.$ = function (obj) { return document.getElementById(obj); } //document.getElementById
global.prototype.$$ = function (obj) { return document.getElementsByName(obj); } //document.getElementsByName /IE
global.prototype.$$$ = function (inputid, node, getattribute) { return document.getElementsByName(inputid)[node].getAttribute(getattribute); } //document.getElementsByName / FOIRFOX
global.prototype.isEmail = function (obj)//是否Email格式
{
    var emailRegExp = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (!emailRegExp.test(obj)) return false; return true;
}
global.prototype.replace = function (obj) {
    return obj.replace(/(^\s*)|(\s*$)/g, "");
}
global.prototype.validinput = function (str)//判断用户输入的值是否包括中文 、长度
{
    var vstr = str;
    var reg_name = /^([a-zA-Z0-9_-]{6,16})+/;
    if (!reg_name.test(vstr)) return false; return true;
}
global.prototype.letters = function (str) {
    if (/^[a-zA-Z0-9_-]+$/.test(str)) { return true; }
    else { return false; }
}
global.prototype.returnajax = function (xmlhttp, XmlString, Url, Func) {
    var xmlString = XmlString;
    var url = Url;
    var func = Func;
    xmlhttp.open("POST", url, true);
    xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xmlhttp.onreadystatechange = func;
    xmlhttp.send(xmlString);
}
global.prototype.replacestr = function (str, symbol, sy)//str字符串，symbol标识符ru（,|_）,sy需要分割的表示符(|)，其中symbol中的|和sy一致
{
    for (var i = 0; i < symbol.split(sy).length; i++) { if (str.indexOf(symbol.split(sy)[i]) > -1) { return false; } }
}

String.prototype.trim = function () { return this.replace(/(^\s*)|(\s*$)/g, " "); }
String.prototype.ltrim = function () { return this.replace(/^\s*/, " "); }
String.prototype.rtrim = function () { return this.replace(/\s*$/, " "); }


function GetList(flag) {
    var xmlhttp = createxmlhttp();
    var strStatus = flag;
    var iSetmax = 10000;
    var xmlString = "strStatus=" + escape(strStatus) + "&iSetmax=" + escape(iSetmax);
    var url = "TipWaiting.aspx";
    g.returnajax(xmlhttp, xmlString, url, updatePage);
    function updatePage() {
        if (xmlhttp.readyState == 4) {
            if (xmlhttp.status == 200) {
                var res = xmlhttp.responseText;
                if (res.length > 1000 || res.indexOf("error") != -1) {
                    //如果返回信息过长，则表明错误或者是超时退出，
                }
                else {
                    document.getElementById("smt_tip").innerHTML = res;
                }

            }
            else { res = "Server Error:" + xmlhttp.statusText; }
        }
    }
}







function StartTimer(flag) {
    GetList(flag);
    //DK:
    //暂时先屏蔽动态获取待办数量
    //setInterval("GetList('" + flag + "')", 5000); //每5秒执行一次
}
//提交
function FormSubmit(obj) {
    obj.submit();
}
function FormReset(obj) {
    obj.reset();
}

//输入字符
function fInputFocus(o) {
    if (o.id == "txtUserName") {
        if (document.getElementById(o.id).value == "用户名") {
            document.getElementById(o.id).value = "";
            document.getElementById(o.id).style.color = "#000000";
        }
    }
    if (o.id == "txtPassword") {
        if (document.getElementById(o.id).value == "密码") {
            document.getElementById(o.id).type = "password";
            document.getElementById(o.id).value = "";
            document.getElementById(o.id).style.color = "#000000";
        }
    }
}


function fInputBlur(o) {
    if (o.id == "txtUserName") {
        if (document.getElementById(o.id).value == "") {
            document.getElementById(o.id).value = "用户名";
            document.getElementById(o.id).style.color = "#aaaaaa";
        }
    }
    if (o.id == "txtPassword") {
        if (document.getElementById(o.id).value == "") {
            document.getElementById(o.id).type = "text";
            document.getElementById(o.id).value = "密码";
            document.getElementById(o.id).style.color = "#aaaaaa";
        }
    }
}


//输入字符
function textInputFocus(o) {

    if (document.getElementById(o.id).value == "审核意见") {
        document.getElementById(o.id).value = "";
        document.getElementById(o.id).style.color = "#000000";
    }

}
function textInputBlur(o) {
    if (document.getElementById(o.id).value == "") {
        document.getElementById(o.id).value = "审核意见";
        document.getElementById(o.id).style.color = "#aaaaaa";
    }

}

function inputFocus(o) {
    var element = document.getElementById(o.id);

    if (element.value != null && element.value != "") {
        document.getElementById(o.id).value = "";
        document.getElementById(o.id).style.color = "#000000";
    }
}

function inputBlur(o, textValue) {
    if (document.getElementById(o.id).value == "") {
        document.getElementById(o.id).value = textValue;
        document.getElementById(o.id).style.color = "#aaaaaa";
    }
}

function Mousemove(obj) {
    // alert(obj.style.backgroundImage);
    // obj.className = "listWaitDiv1";
    //obj.style.backgroundImage = "url(images/List_Background_on.png)";
    // alert(obj.style.backgroundImage);
}
function Mouseout(obj) {
    obj.className = "listWaitDiv";
}
function UserMousemove(obj) {
    obj.className = "listUserDiv1";
}
function UserMouseout(obj) {
    obj.className = "listUserDiv";
}
function MousemoveAudit(obj) {
    document.getElementById(obj).style.background = "url(Images/AuditOn.png)";
}
function MouseoutAudit(obj) {
    document.getElementById(obj).style.background = "url(Images/AuditOff.png)";
}

function MousemoveConsult(obj) {
    document.getElementById(obj).style.background = "url(Images/ConsultOn.jpg)";
}
function MouseoutConsult(obj) {
    document.getElementById(obj).style.background = "url(Images/ConsultOff.jpg)";
}
function AuditYes() {
    //document.FromAudit.action = "Message.aspx";
    document.getElementById("buttonTag").value = "pass";
    document.FromAudit.submit();
}
function AuditNo() {
    //document.FromAudit.action = "Message.aspx";
    document.getElementById("buttonTag").value = "nopass";
    document.FromAudit.submit();
}

function GoHref(element, obj, listtype) {
    Mousemove(element);
    window.location.href = "Bill.aspx?MessageID=" + obj + "&listtype=" + listtype;
}

function GoOrgHref(employeID, category, postname,from) {
    window.location.href = "ListOrg.aspx?EmployID=" + employeID + "&Category=" + category + "&PostName=" + postname+"&from="+from;
}
function GoHrefByNewsDetail(element, obj) {
    Mousemove(element);
    window.location.href = "NewsDetail.aspx?NewsID=" + obj;
}
function GoHrefByEmployeeDetail(element, obj, postid) {
    Mousemove(element);
    window.location.href = "EmployeeDetail.aspx?EmployeeID=" + obj + "&PostID=" + postid;
}
function GoHrefByOfficeDetail(element, obj) {
    Mousemove(element);
    window.location.href = "OfficeDetail.aspx?FormID=" + obj;
}
function GoHrefByRecordDetail(element, obj, checkstate, systemcode, forward, PERSONALRECORDID) {
    Mousemove(element);
    window.location.href = "RecordDetail.aspx?MessageID=" + obj + "&checkstate=" + checkstate + "&SystemCode=" + systemcode + "&forward=" + forward + "&PERSONALRECORDID=" + PERSONALRECORDID;
}
function GoWaited(obj) {
    window.location.href = "BillWaited.aspx?MessageID=" + obj;
}

var t;
var meter;
var timeout = false;
function showmaskdiv() {
    //alert('zz');
    t = new Date();
    document.getElementById("maskbg").style.display = "block";
    document.getElementById("maskshow").style.display = "block";
    document.getElementById("masktimeout").style.display = 'none';
    timeout = false;
    caltime();


}
function hidemaskdiv() {
    document.getElementById("maskbg").style.display = 'none';
    document.getElementById("maskshow").style.display = 'none';
    document.getElementById("masktimeout").style.display = 'none';
    clearTimeout(meter);
}
function caltime() {
    var d = new Date();
    //alert((d - t) > 500);
    //设置2分钟超时
    if ((d - t) > 120000) {
        document.getElementById("maskshow").style.display = 'none';
        document.getElementById("masktimeout").style.display = 'block';
        timeout = true;
    }
    meter = setTimeout(caltime, 100);
}


function setCookies(name, value) {
    var Days = 1; //此 cookie 将被保存 30 天
    var exp = new Date(); //new Date("December 31, 9998");
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";expire=" + exp.toGMTString() + '; path=/';
}

function getCookies(name) {
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (arr != null) return unescape(arr[2]); return null;
}


function delCookies(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 100);
    var cval = getCookie(name);
    if (cval != null) {
        document.cookie = name + "=" + cval.trim() + ";expire=" + exp.toGMTString() + '; path=/';
        setCookies(name, "");
    }
}

function getCookieVal(offset) {
    var endstr = document.cookie.indexOf(";", offset);
    if (endstr == -1) {
        endstr = document.cookie.length;
    }
    return unescape(document.cookie.substring(offset, endstr));
}

function getCookie(name) {
    var arg = name + "=";
    var alen = arg.length;
    var clen = document.cookie.length;
    var i = 0;

    while (i < clen) {
        var j = i + alen;
        if (document.cookie.substring(i, j) == arg) {
            return getCookieVal(j);
        }
        i = document.cookie.indexOf(" ", i) + 1;
        if (i == 0) break;
    }
    return "";

}

