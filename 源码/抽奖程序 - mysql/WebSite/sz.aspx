<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sz.aspx.cs" Inherits="Asd.Award.AwardPrintSZ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AISIDI AWARD 2012</title>
    <script src="Scripts/jquery-1.7.min.js" type="text/javascript"></script>
    <style type="text/css">
        body
        {
            margin: 0;
            padding: 0;
            font-size: 14px;
            font-family: '宋体' ,Tahoma;
        }
        .main
        {
            overflow: auto;
        }
        .chart
        {
            border: 2px solid #999;
            border-top: 0;
            border-right: 0;
            width: 100%;
            overflow: auto;
        }
        .chart td
        {
            height: 300px;
        }
        .chart td, .chart th
        {
            text-align: center;
            position: relative;
        }
        .chart td em
        {
            position: absolute;
            bottom: 0;
            left: 0;
            display: inline-block;
            color: #000;
            text-align: center;
            padding: -30px 0 0 0;
            width: 40px;
            line-height: -300px;
            font-style: normal;
        }
        .chart td em span
        {
            position: absolute;
            margin-top: -15px;
        }
        .chart td em.sz
        {
            background-color: Red;
            margin-left: 10px;
        }
        .chart td em.bj
        {
            background-color: blue;
            margin-left: 60px;
        }
        .chart th span
        {
            position: absolute;
            margin: 10px 0 0 10px;
        }
        ul, li
        {
            list-style-type: none;
            margin: 0;
            padding: 0;
        }
        #tab_menu
        {
            margin: 5px auto;
            border-bottom: 1px solid #000;
            height: 20px;
            padding-left: 20px;
        }
        #tab_menu li
        {
            float: left;
            margin-right: 2px;
            height: 20px;
            padding: 0 5px;
            line-height: 20px;
            text-align: center;
            cursor: pointer;
        }
        #tab_menu li.act
        {
            cursor: default;
            border: 1px solid #000;
            border-bottom: none;
            font-weight: bold;
        }
        #tab_content li
        {
            display: none;
        }
        #tab_content li.act
        {
            display: block;
        }
        table
        {
            width: 280px;
            border-collapse: collapse;
            float: left;
            margin-right: 40px;
        }
        td
        {
            border: 1px solid #999;
            padding: 3px 5px;
            font-size: 16px;
            font-family: Tahoma;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#tab_menu li").click(function () {
                var index = $(this).index();
                $(this).addClass("act").siblings().removeClass("act");
                $("#tab_content").children().eq(index).addClass("act").siblings().removeClass("act");
            })
        })
    </script>
</head>
<body>
    <form id="Form1" runat="server">
    <ul id="tab_menu">
        <li>特等奖</li>
        <li>一等奖</li>
        <li>二等奖</li>
        <li>三等奖</li>
        <li>四等奖</li>
        <li>五等奖</li>
        <li>补抽奖</li>
    </ul>
    <ul id="tab_content">
        <li class="act">
            <table id="table0" runat="server">
                <tr>
                    <th colspan="2">
                        特等奖
                    </th>
                </tr>
            </table>
        </li>
        <li>            
            <table id="table1" runat="server">
                <tr>
                    <th colspan="2">
                        一等奖
                    </th>
                </tr>
            </table>
        </li>
        <li>
            <table id="table2" runat="server">
                <tr>
                    <th colspan="2">
                        二等奖
                    </th>
                </tr>
            </table>
        </li>
        <li>            
            <table id="table3" runat="server">
                <tr>
                    <th colspan="2">
                       三等奖
                    </th>
                </tr>
            </table>
        </li>
        <li>
            <table id="table4" runat="server">
                <tr>
                    <th colspan="2">
                        四等奖
                    </th>
                </tr>
            </table>
            </li>
        <li>
            <table id="table5" runat="server">
                <tr>
                    <th colspan="2">
                        五等奖
                    </th>
                </tr>
            </table>
        </li>
         <li>
            <table id="table6" runat="server">
                <tr>
                    <th colspan="2">
                        补抽奖
                    </th>
                </tr>
            </table>
        </li>
    </ul>
  </form>
</body>
</html>
