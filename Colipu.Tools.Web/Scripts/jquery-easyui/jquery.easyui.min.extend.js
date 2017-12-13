$.extend($.fn.validatebox.defaults.rules, {
    fileExtension: {
        validator: function (value, param) {
            if (value == undefined || value == null || param == undefined || param == null || param.length == 0) return false;
            return value.toLowerCase().lastIndexOf(param[0].toLowerCase()) > 0;
        },
        message: '请选择{0}格式文件!'
    }
});

//datagrid扩展方法,导出数据
$.extend($.fn.datagrid.methods, {
    export: function (jq, ops) {
        if (jq.length == 0) return;
        var defaults = {
            sheetName: "sheet1"
        };
        var options = $.extend(defaults, ops);
        var cols = new Array();
        var frozenColumns = this.options(jq).frozenColumns;
        if (frozenColumns.length != 0) {
            var columns = frozenColumns[frozenColumns.length-1];
            for (var rIndex = 0; rIndex < columns.length; rIndex++) {
                var column = columns[rIndex];
                var col = "{'" + column.field + "':'" + column.title + "'}";
                cols.push(eval('(' + col + ')'));
            }
        }
        var ocolumns = this.options(jq).columns;
        if (frozenColumns.length != 0) {
            var columns = ocolumns[ocolumns.length - 1];
            for (var rIndex = 0; rIndex < columns.length; rIndex++) {
                var column = columns[rIndex];
                var col = "{'" + column.field + "':'" + column.title + "'}";
                cols.push(eval('(' + col + ')'));
            }
        }
        var rows = this.getData(jq).rows;
        var tableHtml = $.json2Table({ columns: cols, rows: rows });
        var excelFile = "<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'>";
        excelFile += "<head>";
        excelFile += "<!--[if gte mso 9]>";
        excelFile += "<xml>";
        excelFile += "<x:ExcelWorkbook>";
        excelFile += "<x:ExcelWorksheets>";
        excelFile += "<x:ExcelWorksheet>";
        excelFile += "<x:Name>";
        excelFile += options.sheetName;
        excelFile += "</x:Name>";
        excelFile += "<x:WorksheetOptions>";
        excelFile += "<x:DisplayGridlines/>";
        excelFile += "</x:WorksheetOptions>";
        excelFile += "</x:ExcelWorksheet>";
        excelFile += "</x:ExcelWorksheets>";
        excelFile += "</x:ExcelWorkbook>";
        excelFile += "</xml>";
        excelFile += "<![endif]-->";
        excelFile += "<style type=\"text/css\">td{mso-number-format:\"\\@\";border:1px;border-style:solid;}</style>"
        excelFile += "</head>";
        excelFile += "<body>";
        excelFile += tableHtml;
        excelFile += "</body>";
        excelFile += "</html>";
        var base64data = "base64," + $.base64.encode(excelFile);
        window.open('data:application/vnd.ms-excel;filename=exportData.doc;' + base64data);
    },
    //ops= { title: "明细", index:0, width: 450, height: 400, modal: false, row: null };
    showPropertyGrid: function (jq, ops) {
        if (jq.length == 0 || ops == null) return;
        var defaults = { title: "明细", index: 1, width: 450, height: 400, modal: false, row: null };
        var options = $.extend(defaults, ops);
        var row = options.row == null ? $(jq).datagrid("getSelected") : options.row
        if (row == null) return;
        var cols = new Array();
        var frozenColumns = this.options(jq).frozenColumns;
        if (frozenColumns.length != 0) {
            var fcolumns = this.options(jq).frozenColumns[frozenColumns.length - 1];
            for (var rIndex = 0; rIndex < fcolumns.length; rIndex++) {
                var column = fcolumns[rIndex];
                var col = { name: column.title, value: row[column.field] }
                cols.push(col);
            }
        }
        var columns = this.options(jq).columns;
        if (columns.length != 0) {
            var ccolumns = this.options(jq).columns[columns.length - 1];
            for (var rIndex = 0; rIndex < ccolumns.length; rIndex++) {
                var column = ccolumns[rIndex];
                var col = { name: column.title, value: row[column.field] }
                cols.push(col);
            }
        }
        var id = "t_w_" + options.index;
        if ($("#" + id).length == 0)
            $("body").append("<div id='" + id + "'><div id='" + id + "1'></div></div>");
        id = "#" + id;
        $(id + "1").propertygrid({
            showGroup: false,
            fit: true,
            fitColumns: true,
            scrollbarSize: 0,
            columns: [
                [
                 { field: 'name', title: '属性', width: 100, sortable: true },
                 { field: 'value', title: '值', width: 100, resizable: false }
                ]],
            data: cols
        });
        $(id).window({
            title: options.title,
            modal: options.modal,
            width: options.width,
            height: options.height,
            minimizable: false,
            collapsible: false,
            top: (options.index * 5),
            left: document.body.clientWidth - options.width,
            closable: true,
            onClose: function () {
                $(id).window("destroy");
            }
        });

    }
});
//form的扩展方法获取json数据
$.extend($.fn.form.methods, {
    //获取json格式数据
    getData: function (jq) {
        var temp = $(jq).serializeArray();
        var json = {};
        for (var i in temp) {
            var row = temp[i];
            json[row.name] = row.value;
        }
        return json;
    }
});

$.extend($.fn.textbox.methods, {
    addClearBtn: function (jq, iconCls) {
        return jq.each(function () {
            var t = $(this);
            var opts = t.textbox('options');
            opts.icons = opts.icons || [];
            opts.icons.unshift({
                iconCls: iconCls,
                handler: function (e) {
                    $(e.data.target).textbox('clear').textbox('textbox').focus();
                    $(this).css('visibility', 'hidden');
                }
            });
            t.textbox();
            if (!t.textbox('getText')) {
                t.textbox('getIcon', 0).css('visibility', 'hidden');
            }
            t.textbox('textbox').bind('keyup', function () {
                var icon = t.textbox('getIcon', 0);
                if ($(this).val()) {
                    icon.css('visibility', 'visible');
                } else {
                    icon.css('visibility', 'hidden');
                }
            });
        });
    }
});



$.extend($.fn.datagrid.defaults, {
    rowHeight: 25,
    onBeforeFetch: function (page) { },
    onFetch: function (page, rows) { }
});

