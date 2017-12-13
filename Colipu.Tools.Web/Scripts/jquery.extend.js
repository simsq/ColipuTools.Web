$.json2Table = (function ($) {
    var json2Table = function (ops) {
        var defaults = {
            columns: [],
            rows: []
        };
        var options = $.extend(defaults, ops);
        if (options.columns.length == 0 && options.rows.length == 0) return "";
        var thead = "<thead><tr>";
        var cols = new Array();
        if (options.columns.length == 0) {
            for (var key in options.rows[0]) {
                thead += "<th>" + key + "</th>"
                cols[cols.length] = key;
            }
        }
        else {
            for (var rIndex = 0; rIndex < options.columns.length; rIndex++) {
                var column = options.columns[rIndex]
                for (var key in column) {
                    console.log(key);
                    thead += "<th>" + column[key] + "</th>"
                    cols[cols.length] = key;
                }
            }
        }
        thead += "</tr></thead>";
        var tbody = "<tbody>"
        for (var rIndex = 0; rIndex < options.rows.length; rIndex++) {
            tbody += "<tr>";
            for (var cIndex = 0; cIndex < cols.length; cIndex++) {
                tbody += "<td>" + options.rows[rIndex][cols[cIndex]] + "</td>";
            }
            tbody += "</tr>";
        }
        tbody += "</tbody>"
        return "<table>" + thead + tbody + "</table>";
    }
    return json2Table;
})(jQuery);


jQuery.cookie = function (key, value, options) {
    // key and value given, set cookie...         
    if (arguments.length > 1 && (value === null || typeof value !== "object")) {
        options = jQuery.extend({}, options);
        if (value === null) {
            options.expires = -1;
        }
        if (typeof options.expires === 'number') {
            var days = options.expires, t = options.expires = new Date();
            t.setDate(t.getDate() + days);
        }
        return (document.cookie = [
            encodeURIComponent(key), '=',
            options.raw ? String(value) : encodeURIComponent(String(value)),
            options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IE
            options.path ? '; path=' + options.path : '',
            options.domain ? '; domain=' + options.domain : '',
            options.secure ? '; secure' : ''
        ].join(''));
    }
    // key and possibly options given, get cookie...
    options = value || {};
    var result, decode = options.raw ? function (s) { return s; } : decodeURIComponent;
    return (result = new RegExp('(?:^|; )' + encodeURIComponent(key) + '=([^;]*)').exec(document.cookie)) ? decode(result[1]) : null;

};

//��ʾȫ��Ψһ��ʶ�� (GUID)��
function Guid(g) {   
    var arr = new Array(); //���32λ��ֵ������
    if (typeof (g) == "string") { //������캯���Ĳ���Ϊ�ַ���
        InitByString(arr, g);
    }
    else {
        InitByOther(arr);
    }
    //����һ��ֵ����ֵָʾ Guid ������ʵ���Ƿ��ʾͬһ��ֵ��
    this.Equals = function (o) {
        if (o && o.IsGuid) {
            return this.ToString() == o.ToString();
        }
        else {
            return false;
        }
    }
    //Guid����ı��

    this.IsGuid = function () { }
    //���� Guid ��Ĵ�ʵ��ֵ�� String ��ʾ��ʽ��      
    this.ToString = function (format) {
        if (typeof (format) == "string") {
            if (format == "N" || format == "D" || format == "B" || format == "P") {
                return ToStringWithFormat(arr, format);
            }
            else {
                return ToStringWithFormat(arr, "D");
            }
        }

        else {
            return ToStringWithFormat(arr, "D");
        }
    }
    //���ַ�������         
    function InitByString(arr, g) {
        g = g.replace(/\{|\(|\)|\}|-/g, "");
        g = g.toLowerCase();
        if (g.length != 32 || g.search(/[^0-9,a-f]/i) != -1) {
            InitByOther(arr);
        }
        else {
            for (var i = 0; i < g.length; i++) {
                arr.push(g[i]);
            }
        }
    }
    //���������ͼ���       
    function InitByOther(arr) {
        var i = 32;
        while (i--) {
            arr.push("0");
        }
    }
    /* 
    �������ṩ�ĸ�ʽ˵���������ش� Guid ʵ��ֵ�� String ��ʾ��ʽ��
    N  32 λ�� xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx                   
    D  �����ַ��ָ��� 32 λ���� xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
    B  ���ڴ������С������ַ��ָ��� 32 λ���֣�{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
    P  ����Բ�����С������ַ��ָ��� 32 λ���֣�(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)
    */
    function ToStringWithFormat(arr, format) {
        switch (format) {
            case "N":
                return arr.toString().replace(/,/g, "");
            case "D":
                var str = arr.slice(0, 8) + "-" + arr.slice(8, 12) + "-" + arr.slice(12, 16) + "-" + arr.slice(16, 20) + "-" + arr.slice(20, 32);
                str = str.replace(/,/g, "");
                return str;
            case "B":
                var str = ToStringWithFormat(arr, "D");
                str = "{" + str + "}";
                return str;
            case "P":
                var str = ToStringWithFormat(arr, "D");
                str = "(" + str + ")";
                return str;
            default:
                return new Guid();
        }
    }
}
//Guid ���Ĭ��ʵ������ֵ��֤��Ϊ�㡣
Guid.Empty = new Guid();
//��ʼ�� Guid ���һ����ʵ����
Guid.NewGuid = function () {
    var g = "";
    var i = 32;
    while (i--) {
        g += Math.floor(Math.random() * 16.0).toString(16);
    }
    return new Guid(g);

}