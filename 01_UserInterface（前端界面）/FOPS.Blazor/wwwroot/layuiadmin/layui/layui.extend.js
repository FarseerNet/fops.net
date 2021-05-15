// layuiadmin的擴展方法
// 常用工具方法封裝
if (!window["Utils"]) window["Utils"] = new Object();
!function (ns) {
    // 常用方法封装
    ns["wait"] = function (name, data, faildCount) {
        var obj = this;
        if (!faildCount) faildCount = 0;
        if (faildCount > 10) {
            layer.msg(name + "超過重試次數", { icon: 2, time: 1000 });
            return;
        }
        var callback = typeof name === "function" ? name : BW.callback[name];

        if (callback) {
            callback = callback.bind(obj);
            callback(data);
        } else {
            setTimeout(function () {
                Utils.wait(name, data, faildCount + 1);
            }, 250);
        }
    };
    // 自動過濾標籤數組
    ns["gettag"] = function (type, parentId) {
        if (!GolbalSetting.tag) return [];
        if (!parentId) parentId = 0;
        if (!GolbalSetting.tag[type]) {
            layer.msg("標籤類型" + type + "不存在", { icon: 2 });
            return false;
        }
        return GolbalSetting.tag[type].filter(function (item) { return item.ParentID.toString() === parentId.toString(); });
    };
    // 获取枚举值
    ns["getenum"] = function (name, value) {
        if (!name || !value) return "N/A";
        if (!GolbalSetting.enum[name]) return value;
        var result = [];
        var values = value.split(',');
        for (var i = 0; i < values.length; i++) {
            var key = values[i].replace(/\s+/, "");
            result.push(GolbalSetting.enum[name][key] || key);
        }
        return result.join(",");
    };

    // 格式化
    ns["format"] = function (type, value) {
        if (!window["htmlFunction"] || !htmlFunction[type]) return value;
        return htmlFunction[type](value);
    };

    // 生成一個Guid格式的隨機字符串
    ns["NewGuid"] = function (format) {
        switch (format) {
            case "N":
            case "n":
                format = "xxxxxxxxxxxx4xxxyxxxxxxxxxxxxxxx";
                break;
            default:
                format = "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx";
                break;
        }
        return format.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    };

    // 当前是否是手机端
    ns["mobile"] = function () {
        var userAgentInfo = navigator.userAgent;
        var agents = new Array("Android", "iPhone", "SymbianOS", "WP", "iPad", "iPod");
        var data = {};
        var flag = false;
        for (var i = 0; i < agents.length; i++) {
            if (userAgentInfo.indexOf(agents[i]) > 0) {
                flag = true;
                data[agents[i]] = true;
                break;
            }
        }
        if (/MicroMessenger/i.test(userAgentInfo)) {
            flag = true;
            data["Wechat"] = true;
        }
        if (!flag) return null;
        if (data["iPhone"] || data["iPad"] || data["iPod"]) data["ios"] = true;
        return data;
    };

    // 播放声音
    ns["Sound"] = function (sound) {
        var id = "betwin-soundplayer";
        var audio = document.getElementById(id);
        if (!audio) {
            audio = document.createElement("audio");
            audio.id = id;
            document.body.insertBefore(audio, null);
        }
        if (sound.indexOf("/") === -1) sound = "/studio/sound/" + sound + ".mp3";
        audio.src = sound;
        audio.play();
        //audio.addEventListener("ended", function (e) { document.body.removeChild(audio); });
        //audio.addEventListener("error", function (e) { document.body.removeChild(audio); });
    };

    // 文字语音
    ns["SoundText"] = function (text) {
        return "//fanyi.baidu.com/gettts?lan=zh&spd=5&source=web&text=" + text;
    };

    // 导入动态资源
    ns["link"] = function (path) {
        var doc = document;
        // 收藏夹图标
        if (/favicon.ico/.test(path)) {
            var head = doc.getElementsByTagName('head')[0];
            var shortcut = doc.createElement('link');
            shortcut.rel = "shortcut icon";
            shortcut.href = path;
            var bookmark = doc.createElement("link");
            bookmark.rel = "bookmark";
            bookmark.href = path;
            bookmark.type = "image/x-icon";
            head.appendChild(shortcut);
            head.appendChild(bookmark);
        }
    };

    // 信息通知（自动调用桌面通知）
    ns["Notify"] = function (info) {
        if (window["Notification"] && Notification.permission !== 'denied' && Notification.permission !== 'granted') {
            Notification.requestPermission(function (permission) {
                if (permission === "granted") {
                    layer.msg("您已成功开启桌面通知功能");
                }
            });
        }
        if (window["Notification"] && Notification.permission === "granted") {
            new Notification(info.title, {
                body: info.body,
                icon: info.icon
            });
        } else {
            if (info.icon) info.body += "<i class='layui-layer-notify-icon' style='background-image:url(\"" + info.icon + "\");'></i>";
            layer.open({
                id: "notify-" + new Date().getTime(),
                title: info.title,
                skin: "layui-layer-notify",
                content: info.body,
                offset: 'rb',
                closeBtn: 1,
                type: 1,
                shade: 0,
                time: 15000,
                anim: 2,
                btn: null
            });
        }
        if (info.sound) ns.Sound(info.sound);
    };

    // 订阅推送频道
    ns["Subscribe"] = function (setting, channel, callback) {
        // 使用 Pusher
        if (window["Pusher"]) {
            if (!callback) callback = {};
            if (GolbalSetting.Pusher) {
                layui.each(GolbalSetting.Pusher.channels.channels, function (name) {
                    GolbalSetting.Pusher.unsubscribe(name);
                });
            } else {
                GolbalSetting.Pusher = new Pusher(setting.key, {
                    cluster: setting.cluster,
                    forceTLS: true
                });
            }
            layui.each(channel, function (name, event) {
                // name：频道名字
                var obj = GolbalSetting.Pusher.subscribe(name);
                obj.bind(event, function (data) {
                    if (callback[event]) {
                        callback[event](data);
                    } else if (callback[name]) {
                        callback[name](data);
                    } else {
                        ns.Notify({
                            title: data.Title,
                            body: data.Message,
                            icon: "/images/notify/" + event + ".png",
                            sound: Utils.SoundText(data.Message)
                        });
                    }
                });
            });
        }

        // 使用 GoEasy 通道
        if (window["GoEasy"]) {
            if (GolbalSetting.Pusher) {
                layui.each(GolbalSetting.Pusher.channels.channels, function (name) {
                    GolbalSetting.Pusher.unsubscribe(name);
                });
            } else {
                GolbalSetting.Pusher = new GoEasy({
                    host: setting.host || 'hangzhou.goeasy.io',//应用所在的区域地址: 【hangzhou.goeasy.io |singapore.goeasy.io】
                    appkey: setting.appkey,
                    forceTLS: true,
                    userId: setting.userId || null, //必须指定，不然无法实现客户端上下线监听功能
                    userData: setting.userData || "" //更多的用户信息，其它已监听上下线信息的用户，收到该用户上线信息里会包含此部分内容
                });
            }
            layui.each(channel, function (name, event) {
                // name：频道名字    event：该频道代表的动作
                GolbalSetting.Pusher.subscribe({
                    channel: name,
                    onMessage: function (message) {
                        var data = JSON.parse(message.content);
                        if (callback[event]) {
                            callback[event](data);
                        } else if (data.Title && data.Message) {
                            ns.Notify({
                                title: data.Title,
                                body: data.Message,
                                icon: "/images/notify/" + event + ".png",
                                sound: Utils.SoundText(data.Message)
                            });
                        }
                    }
                });
            });
        }
    };

    // 公共错误页面
    ns["ErrorPage"] = function (res) {
        if (res.info && res.info.ErrorType) {
            location.href = '/studio/layadmin/views/error/' + res.info.ErrorType.toLowerCase() + ".html?" + encodeURI(JSON.stringify(res.info));
        } else {
            layer.alert(res.msg, { icon: 2 });
        }
    };

    // 枚举选择器
    ns["EnumSelect"] = function (type, save, options) {
        if (!GolbalSetting.enum || !GolbalSetting.enum[type]) {
            layer.msg("枚举类型" + type + "不存在", {
                icon: 2
            });
            return;
        }
        if (!options) options = {};
        if (!options.maxWidth) options.maxWidth = 320;
        if (!options.maxHeight) options.maxHeight = 240;

        var filterId = "enumselect-" + new Date().getTime();
        var submitId = filterId + "-submit";
        var html = ["<form class='layui-form' lay-filter='" + filterId + "'>", "<div class='layui-form-item'>"];
        layui.each(GolbalSetting.enum[type], function (key, name) {
            html.push("<input type='radio' name='Type' value='" + key + "' title='" + name + "'" + (options.value === key ? "checked" : "") + " />");
        });
        html.push("</div>");
        html.push("<div class='layui-form-item'><button class='layui-btn' lay-submit lay-filter='" + submitId + "'>确定</button></div>");
        html.push("</form>");

        options.content = html.join("");
        options.btn = [];

        layui.use("form", function () {
            var form = layui.form;
            var index;
            if (!options.success) options.success = function () {
                form.render(null, filterId);
                form.on("submit(" + submitId + ")", function (e) {
                    var type = e.field.Type;
                    if (type) {
                        layer.close(index);
                        save(type);
                    }
                    return false;
                });
            };
            index = layer.open(options);
        });
    };

    // 缓存读取资源，异步更新缓存
    ns["req"] = function (options) {
        var key = (options.url || "") + "?" + (options.data ? JSON.stringify(options.data) : "");
        if (window["md5"]) key = md5(key);
        var res = null;
        if (window["localStorage"]) {
            var result = localStorage.getItem(key);
            if (result) {
                try {
                    res = JSON.parse(result);
                    if (!res || !res.success) res = null;
                } catch (ex) {
                    res = null;
                }
            }
            if (res) {
                options.success(res);
                options.success = function (res) {
                    if (res.success) {
                        localStorage.setItem(key, JSON.stringify(res));
                    }
                };
            } else {
                console.log("没有本地资源");
                var success = options.success || function () { };

                options.success = function (res) {
                    success(res);
                    if (res.success) {
                        localStorage.setItem(key, JSON.stringify(res));
                    }
                };
            }
        }
        layui.$.ajax(options);
    };

    // 封装JSON的视图显示
    ns["jsonViewer"] = function (elemId, json) {
        if (!window["jQuery"]) window["jQuery"] = layui.$;
        var elem = jQuery("#" + elemId);
        layui.link("//studio.racdn.com/js/jquery.json-viewer.css");
        if (elem.jsonViewer) {
            elem.jsonViewer(json);
        } else {
            jQuery.getScript("//studio.racdn.com/js/jquery.json-viewer.js", function () {
                elem.jsonViewer(json);
            });
        }
    };

    // 通过全局的点击事件监听，设定当前会员在线
    ns["Online"] = function (options) {
        if (!options.url) {
            console.error("未指定在线状态通知接口");
            return;
        }
        if (!options["time"]) options["time"] = 10 * 1000;
        var lastTime = 0;
        document.body.addEventListener("click", function () {
            if (!layui || !layui.admin) return;
            var now = new Date().getTime();
            if (lastTime + options.time > now) return;
            lastTime = now;
            layui.admin.req({
                url: options.url,
                dataType: "text",
                type: "put"
            });
        });
    };

    //　获取配置参数值，转化成为QueryString|json 字符串
    ns["GetSetting"] = function (data, prefix, field, type) {
        if (!field) field = prefix.substr(0, prefix.length - 1);
        if (!type) type = "query";
        var query = [];
        var json = {};
        var regex = new RegExp("^" + prefix);
        layui.each(data, function (key, value) {
            if (regex.test(key)) {
                var name = key.replace(regex, "");
                delete data[key];
                switch (type) {
                    case "query":
                        query.push(name + "=" + encodeURIComponent(value));
                        break;
                    case "json":
                        json[name] = value;
                        break;
                }
            }
        });
        switch (type) {
            case "query":
                data[field] = query.join("&");
                break;
            case "json":
                data[field] = JSON.stringify(json);
                break;
        }
        return data;
    };

    // 转化成为键值对字符串
    ns["toQueryString"] = function (data) {
        var arr = [];
        for (var key in data) {
            arr.push(key + "=" + data[key]);
        }
        return arr.join("&");
    };

    // 设置或者获取当前的语言包
    ns["Language"] = function (language) {
        // 获取语言包
        if (!language) {
            language = localStorage.getItem("_LANGUAGE") || "CHN";
            return language;
        } else {
            localStorage.setItem("_LANGUAGE", language);
        }
    };

}(Utils);

// 全局設置
var GolbalSetting = {
    "Category": null,
    "enum": null,
    "tag": null,
    // 自定义的页面重写路由
    "router": {},
    // 固定的弹出框大小
    area: {
        xs: ["420px", "360px"],
        sm: ["640px", "480px"],
        md: ["800px", "600px"],
        lg: ["1024px", "768px"],
        xl: ["1280px", "800px"],
        full: ["100%", "100%"]
    }
};

(function (ns) {
    ns.templet = {
        control: function (options) {
            if (!options) options = {};
            var html = [];
            html.push("<div>");
            for (var type in options) {
                switch (type) {
                    case "edit":
                        if (typeof options.edit === "string") options.edit = { action: options.edit };
                        html.push("<button class='layui-btn layui-btn-normal layui-btn-xs' lay-event='edit' ");
                        for (var editKey in options.edit) html.push("data-" + editKey + "='" + options.edit[editKey] + "' ");
                        html.push(" title='" + (options.edit.tip || "修改") + "'><i class='" + (options.edit.icon || "am-icon-edit") + "'></i></button>");
                        break;
                    case "delete":
                        if (typeof options.delete === "string") options.delete = { action: options.delete };
                        html.push("<button class='layui-btn layui-btn-danger layui-btn-xs' lay-event='delete' ");
                        for (var deleteKey in options.delete) html.push("data-" + deleteKey + "='" + options.delete[deleteKey] + "' ");
                        html.push(" title='删除'><i class='am-icon-times'></i></button>");
                        break;
                    default:
                        var item = options[type];
                        html.push("<button class='layui-btn layui-btn-xs " + (item.class || "") + "' lay-event='" + (item.event || "edit") + "' ");
                        html.push(" data-title='" + (item.tip || item.title || item.text || "") + "'");
                        for (var key in item) {
                            html.push(" data-" + key + "='" + item[key] + "'");
                        }
                        if (item.title) html.push(" title='" + item.title + "'");
                        html.push("><i class='" + (item.icon || "") + "'></i> " + (item.text || "") + "</button>");
                        break;
                }
            }
            html.push("</div>");
            return html.join("");
        }
    };

    if (Utils.mobile()) {
        for (var key in ns.area) {
            ns.area[key] = ["100%", "100%"];
        }
    } else {
        var cx = document.documentElement.clientWidth;
        var ch = document.documentElement.clientHeight;

        for (var areaKey in ns.area) {
            var w = ns.area[areaKey][0];
            var h = ns.area[areaKey][1];
            if (cx <= parseInt(w) || ch <= parseInt(h)) {
                ns.area[areaKey] = ["100%", "100%"];
            }
        }
    }

})(GolbalSetting);


// 字符串格式化
if (!window["htmlFunction"]) window["htmlFunction"] = new Object();
!function (ns) {
    ns["money"] = function (value) {
        if (!value) return "<label class='layui-text-black'>0.00</label>";
        var num = Number(value);
        if (isNaN(num)) return value;
        if (num > 0) {
            return "<label class='layui-text-green'>+" + ns["number"](value) + "元</label>";
        } else if (num < 0) {
            return "<label class='layui-text-red'>" + ns["number"](value) + "元</label>";
        }
        return "<label class='layui-text-black'>0.00元</label>";
    };

    ns["bool"] = function (value) {
        if (value) {
            return "<label class='layui-text-green'>是</label>";
        } else {
            return "<label class='layui-text-red'>否</label>";
        }
    };

    ns["datetime"] = function (value) {
        if (typeof value === "object" && value instanceof Date) {
            var date = value;
            return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + " " + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
        }
        if (typeof value === "number") {
            var date = new Date(value);
            return date.formatDate("yyyy-MM-dd hh:mm");
        }
        if (!value || /^1900/.test(value)) return "N/A";
        if (/:\d{2}$/.test(value)) {
            value = value.replace(/(\d{1,2}):(\d{1,2}):\d{2}$/, "$1:$2");
            value = value.replace(/\d+/g, function ($) { return $.length === 1 ? "0" + $ : $ });
            value = value.replace(/\//g, "-");
        }
        return value;
    };

    // laydate 控件要求的日期格式
    ns["laydate"] = function (value, format) {
        if (!value || /^1900/.test(value)) return "N/A";
        if (/^0000/.test(value)) return "";
        if (!format) format = "datetime";
        switch (format) {
            case "datetime":
                format = "yyyy-MM-dd hh:mm:ss";
                break;
            case "date":
                format = "yyyy-MM-dd";
                break;
        }
        if (value instanceof Date) {
            value = value.formatDate(format);
        } else if (typeof value === "number") {
            try {
                if (value < 2649600000) value *= 1000;
                var date = new Date(value);
                value = date.formatDate(format);
            } catch (ex) {
                if (/:\d{2}$/.test(value)) {
                    value = value.replace(/\d+/g, function ($) { return $.length === 1 ? "0" + $ : $; });
                    value = value.replace(/\//g, "-");
                }
            }
        }
        return value;
    };

    // 只有月日的日期格式
    ns["mmdd"] = function (value) {
        if (!value || /^1900/.test(value)) return "N/A";
        var regex = /\/(\d{1,2})\/(\d{1,2})/;
        if (!regex.test(value)) return value;
        return regex.exec(value)[1] + "-" + regex.exec(value)[2];
    };

    ns["time"] = function (value) {
        if (!value || /^1900/.test(value)) return "N/A";
        if (/\d{2}:.+$/.test(value)) return /\d{2}:.+$/.exec(value);
        return value;
    };
    ns["img"] = function (value) {
        if (!value) return "";
        return "<img src='" + value + "'/>";
    };
    ns["n"] = function (value) {
        if (!value) return "0.00";
        var num = Number(value);
        if (isNaN(num)) return value;
        return num.toFixed(2);
    };

    // 带三位分割的数字类型输出（保留两位小数）
    ns["number"] = function (value, dec) {
        if (!value) return "0.00";
        var num = Number(value);
        if (isNaN(num)) return value;
        if (dec === undefined) dec = 2;
        var arr = [];
        var decimal = Math.abs(num) % 1;
        var negative = "";
        if (num < 0) {
            negative = "-";
        }
        var numString = Math.floor(Math.abs(num)).toString();
        var start = numString.length % 3;
        if (start) arr.push(numString.substr(0, start));
        for (var i = 0; i < Math.floor(numString.length / 3); i++) {
            arr.push(numString.substr(i * 3 + start, 3));
        }
        return negative + arr.join(",") + (dec ? (Math.floor(decimal * Math.pow(10, dec)) / Math.pow(10, dec)).toFixed(dec).substr(1) : "");
    };

    ns["enum"] = function (name) {
        if (!GolbalSetting.enum || !GolbalSetting.enum[name]) return;
        var options = new Array();
        layui.each(GolbalSetting.enum[name], function (key, value) {
            options.push("<option value='" + key + "'>" + value + "</option>");
        });
        return options.join("");
    };

    ns["p"] = function (value) {
        var num = Number(value);
        if (isNaN(num)) return value;
        return (num * 100).toFixed(2) + "%";
    };

    ns["date"] = function (value) {
        if (typeof value === "object" && value instanceof Date) {
            var date = value;
            return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
        }
        if (/^1900/.test(value)) return "N/A";
        var regex = /^\d+[/-]\d+[/-]\d+/;
        if (!regex.test(value)) return value;
        return regex.exec(value)[0];
    };

    // 中文表达的年月日
    ns["longdate"] = function (value) {
        if (/^1900/.test(value)) return "N/A";
        var regex = /^(\d+)[/-](\d+)[/-](\d+)/;
        if (!regex.test(value)) return value;
        var result = regex.exec(value);
        return result[1] + "年" + result[2] + "月" + result[3] + "日";
    };

    ns["quot"] = function (html) {
        return html.replace(/\"/g, "&quot;");
    };

    // 显示提示图标
    ns["tip"] = function (tip) {
        if (!tip) return "";
        return "<i class='am-icon-warning layui-text-orange' lay-tips='" + tip + "' lay-offset='-15'></i>";
    };

    // ticks时间戳转换成为日期
    ns["timeticks"] = function (time) {
        var date = new Date((time - 621355968000000000 - 288000000000) / 10000);
        Y = date.getFullYear() + '-';
        M = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '-';
        D = date.getDate() + ' ';
        h = date.getHours() + ':';
        m = date.getMinutes() + ':';
        s = date.getSeconds();
        return Y + M + D + h + m + s;
    };

    // 通用的状态样式
    ns["status"] = function (type, status) {
        if (!type || !status) return "N/A";
        var result = null;
        var name = status;
        if (type.indexOf(".") === -1) {
            status = type;
        } else {
            if (!GolbalSetting.enum[type]) return status;
            name = GolbalSetting.enum[type][status];
            if (!name) return status;
        }
        result = name;
        var style = { color: "", icon: "" };
        //Stop: "停止", Normal: "正常", Maintain: "维护"
        switch (status) {
            case "Normal":
            case "Pass":
            case "Open":
                style.color = "green";
                style.icon = "layui-icon layui-icon-ok-circle";
                break;
            case "Reject":
            case "Close":
                style.color = "red";
                style.icon = "layui-icon layui-icon-close-fill";
                break;
            case "Stop":
                style.color = "red";
                style.icon = "am-icon-lock";
                break;
            case "Maintain":
                style.color = "blue";
                style.icon = "layui-icon layui-icon-util";
                break;
            case "Member":
                style.color = "green";
                style.icon = "layui-icon layui-icon-friends";
                break;
            case "Agent":
                style.color = "green";
                style.icon = "layui-icon layui-icon-group";
                break;
            case "IN":
                style.color = "green";
                style.icon = "am-icon-sign-in";
                break;
            case "OUT":
                style.color = "blue";
                style.icon = "am-icon-sign-out";
                break;
            case "Win":
            case "WinHalf":
                style.color = "green";
                style.icon = "layui-icon layui-icon-face-smile";
                break;
            case "Loss":
            case "Lose":
            case "LossHalf":
                style.color = "red";
                style.icon = "layui-icon layui-icon-face-cry";
                break;
            case "Wait":
            case "None":
                style.color = "gray";
                style.icon = "layui-icon layui-icon-loading layui-anim layui-anim-rotate layui-anim-loop";
                break;
            case "Settlement":
                style.color = "blue";
                style.icon = "layui-icon layui-icon-loading layui-anim layui-anim-rotate layui-anim-loop";
                break;
            case "Revoke":
            case "Cancel":
            case "Draw":
                style.color = "black";
                style.icon = "layui-icon layui-icon-face-surprised";
                break;
            case "PC":
                style.color = "black";
                style.icon = "am-icon-laptop";
                break;
            case "Mobile":
                style.color = "black";
                style.icon = "am-icon-mobile";
                break;
            case "Lock":
                style.color = "red";
                style.icon = "am-icon-lock";
                break;
        }
        if (style.color && style.icon) {
            result = "<span class='layui-text-" + style.color + "'><i class='" + style.icon + "'></i> " + name + "</span>";
        }
        return result;
    };

    // 显示IP
    ns.IP = function (ip, address) {
        if (!/^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$/.test(ip)) return "N/A";
        var result = ["<span>" + ip + "</span>"];
        if (address) result.push("<i class='am-icon-warning layui-text-orange' lay-tips='" + address + "' lay-offset='-15'></i>");
        return result.join(" ");
    };

    // 显示来源平台
    ns.platform = function (platform) {
        return platform;
    };

    // 显示货币
    ns.currency = function (value, currency) {
        var num = value;
        if (typeof value === "string") {
            num = value.replace(/[^\d\.\-]/g, "");
            num = Number(value);
        }
        var tips = null;
        if (currency !== "CNY" && GolbalSetting.exchange && GolbalSetting.exchange[currency]) {
            tips = "";
        }
    };

}(htmlFunction);


// 字符串擴展
!function () {
    String.prototype.toQRCode = function (width, height) {
        if (!width) width = 220;
        if (!height) height = 220;
        var content = this;
        return "https://api.a8.to/qrcode?content=" + encodeURIComponent(content);
    };

    // 从ID中直接获取内容
    String.prototype.innerHTML = function (content) {
        var id = this;
        var obj = document.getElementById(id);
        if (!obj) return null;
        if (content) {
            obj.innerHTML = content;
        } else {
            return obj.innerHTML;
        }
    };

    // 把非格式化的字符串转化成为整形
    String.prototype.toInt = function () {
        var str = /\d+/.exec(this);
        if (str) return Number(str);
        return null;
    };

    // 获取API跨域请求的绝对路径
    String.prototype.getAPIUrl = function () {
        var str = this;
        if (/^http|^\/\//.test(str)) return str;
        if (window["layui"] && layui.cache && layui.cache.apiurl) {
            return layui.cache.apiurl + str;
        }
        return str;
    };

    // 左侧拼凑
    String.prototype.padLeft = function (totalLength, char) {
        var str = this;
        var length = str.length;
        if (totalLength <= length) return str;
        var arr = [];
        for (var i = 0; i < totalLength - length; i++) {
            arr.push(char);
        }
        return arr.join("") + str;
    };
}();

// 元素扩展
!function () {
    // 數組或者物件綁定到select對象
    Element.prototype.select = function (data, key, value, defaultValue) {
        var obj = this;
        var selectedValue = obj.getAttribute("data-value");
        obj.innerHTML = "";
        var options = new Array();
        if (defaultValue) {
            if (defaultValue.indexOf("<") !== 0) defaultValue = "<option value=''>" + defaultValue + "</option>";
            options.push(defaultValue);
        }

        var getKey = function (item) {
            if (typeof key === "string") return item[key];
            return key(item);
        };

        var getValue = function (item) {
            if (typeof value === "string") return item[value];
            return value(item);
        };

        switch (typeof data) {
            case "string":
                var $ = layui.$;
                $.ajax({
                    url: data.getAPIUrl(),
                    method: "post",
                    headers: layui.data(layui.setter.tableName),
                    data: obj.getAttribute("lay-data") === null ? {} : JSON.parse(obj.getAttribute("lay-data")),
                    success: function (result) {
                        if (!result.success) {
                            layer.msg(result.msg, { icon: 2 });
                            return;
                        }
                        if (result.info.list) {
                            obj.select(result.info.list, key, value, defaultValue);
                        } else {
                            obj.select(result.info, key, value, defaultValue);
                        }
                    }
                });
                break;
            default:
                switch (Array.isArray(data)) {
                    case true:
                        layui.each(data, function (index, item) {
                            var itemKey = typeof key === "function" ? key(item) : item[key];
                            var itemValue = typeof value === "function" ? value(item) : item[value];
                            options.push("<option value=\"" + itemKey + "\"" + (itemKey === false ? " disabled" : "") + (selectedValue && (selectedValue === itemKey.toString()) ? " selected" : "") + ">" + itemValue + "</option>");
                        });
                        break;
                    case false:
                        layui.each(data, function (keyData) {
                            var valueData = data[keyData];
                            if (typeof valueData === "string") {
                                var itemKey = key ? (typeof key === "function" ? key(keyData) : keyData[key]) : keyData;
                                var itemValue = value ? (typeof value === "function" ? value(valueData) : valueData[value]) : valueData;
                                switch (obj.getAttribute("data-type")) {
                                    case "radio":
                                        options.push("<input type='radio' name='" + obj.getAttribute("data-name") + "' value=\"" + itemKey + "\" title='" + itemValue + "'" + (selectedValue && selectedValue === itemKey ? " checked" : "") + " />");
                                        break;
                                    default:
                                        options.push("<option value=\"" + itemKey + "\"" + (selectedValue && selectedValue === itemKey.toString() ? " selected" : "") + ">" + itemValue + "</option>");
                                        break;
                                }
                            } else {
                                var groupName = keyData;
                                options.push("<optgroup label=\"" + groupName + "\">");
                                if (Array.isArray(valueData)) {
                                    layui.each(valueData, function (index, item) {
                                        options.push("<option value=\"" + getKey(item) + "\"" + (selectedValue && selectedValue === getKey(item).toString() ? " selected" : "") + ">" + getValue(item) + "</option>");
                                    });
                                } else {
                                    layui.each(valueData, function (itemKey, itemValue) {
                                        options.push("<option value=\"" + itemKey + "\"" + (selectedValue && selectedValue === itemKey.toString() ? " selected" : "") + ">" + itemValue + "</option>");
                                    });
                                }
                                options.push("</optgroup>");
                            }
                        });
                        break;
                }
                obj.innerHTML = options.join("");
                if (obj.getAttribute("lay-done")) {
                    BW.callback.fire.bind(obj)(obj.getAttribute("lay-done"));
                }
                break;
        }
    };

    // 把对象上的data-属性转化成为object参数
    // prefix 前缀，默认为 data-
    Element.prototype.getData = function (prefix) {
        if (!prefix) prefix = "data-";
        var obj = this;
        var data = {};
        for (var i = 0; i < obj.attributes.length; i++) {
            var item = obj.attributes[i];
            var name = item.name;
            if (name.indexOf(prefix) === -1) continue;
            var key = name.substr(prefix.length);
            data[key] = item.value;
        }
        return data;
    };

    // 获取上层元素
    Element.prototype.getParent = function (where, parent) {
        var obj = this;
        if (typeof where === "string") {
            var query = where;
            where = function (dom) {
                return dom.classList && dom.classList.contains(query);
            };
        }
        while (obj) {
            if (where(obj)) return obj;
            if (parent && parent === obj) break;
            obj = obj.parentNode;
        }
        return null;
    };

    // 获取表单元素的值
    Element.prototype.getField = function (attributeName) {
        if (!attributeName) attributeName = "name";
        var elem = this;
        var data = {};
        elem.querySelectorAll("[" + attributeName + "]").forEach(function (item) {
            var name = item.getAttribute(attributeName);
            if (!name) return;
            switch (item.tagName) {
                case "SELECT":
                case "TEXTAREA":
                    data[name] = item.value;
                    break;
                case "INPUT":
                    switch (item.getAttribute("type")) {
                        case "radio":
                            if (item.checked) data[name] = item.value;
                            break;
                        case "checkbox":
                            if (item.checked) {
                                if (!data[name]) data[name] = [];
                                data[name].push(item.value);
                            }
                            break;
                        default:
                            data[name] = item.value;
                            break;
                    }
                    break;
            }
        });
        for (var key in data) {
            if (Array.isArray(data[key])) data[key] = data[key].join(",");
        }
        return data;
    };

    // 获取表单对象
    Element.prototype.getFormElements = function () {
        var elem = this;
        var data = {};
        elem.querySelectorAll("[name]").forEach(function (item) {
            var name = item.getAttribute("name");
            if (!name) return;
            switch (item.tagName) {
                case "SELECT":
                case "TEXTAREA":
                    data[name] = item;
                    break;
                case "INPUT":
                    switch (item.getAttribute("type")) {
                        case "checkbox":
                        case "radio":
                            if (!data[name]) data[name] = [];
                            data[name].push(item);
                            break;
                        default:
                            data[name] = item;
                            break;
                    }
                    break;
            }
        });
        return data;
    };

    // 获取多选框选中的值（返回数组）
    Element.prototype.getCheckbox = function (name) {
        var list = this.querySelectorAll("input[type='checkbox'][name='" + name + "']:checked");
        var result = [];
        for (var i = 0; i < list.length; i++) {
            result.push(list[i].value);
        }
        return result;
    };

    // 通过一个元素数组获取对象
    NodeList.prototype.toObject = function (name, getValue) {
        var getName = null;
        var nodeList = this;
        if (typeof name === "string") {
            getName = function (obj) {
                return obj.getAttribute(name);
            };
        } else {
            getName = name;
        }
        var data = {};
        for (var i = 0; i < nodeList.length; i++) {
            var node = nodeList[i];
            data[getName(node)] = getValue(node);
        }
        return data;
    };

    // 元素转为 数组
    NodeList.prototype.toArray = function () {
        var arr = [];
        this.forEach(t => arr.push(t));
        return arr;
    };

    // 替换元素（名字必须相同）
    Element.prototype.replace = function (newElem) {
        var elem = this;
        if (elem.tagName !== newElem.tagName) {
            console.error("TagName不一致");
            return;
        }
        //#1 清除原来的属性内容
        for (var i = 0; i < elem.attributes.length; i++) {
            var attributeName = elem.attributes[i].name;
            elem.removeAttribute(attributeName);
        }
        //#2 写入新的属性内容
        for (var i = 0; i < newElem.attributes.length; i++) {
            var att = newElem.attributes[i];
            elem.setAttribute(att.name, att.value);
        }
        //#3 内容
        elem.innerHTML = newElem.innerHTML;
        return elem;
    };

    // 把元素插入顶部
    Element.prototype.insertChild = function (newElem) {
        var elem = this;
        var child = elem.querySelector("*");
        if (child) {
            elem.insertBefore(newElem, child);
        } else {
            elem.appendChild(newElem);
        }
        return newElem;
    }

    Element.prototype.toggleClass = function (className) {
        if (!className) return;
        var elem = this;
        if (!elem.classList.contains(className)) {
            elem.classList.add(className);
        } else {
            elem.classList.remove(className);
        }
    };

}();

// 数组扩展
!function () {
    // 判断元素里面是否包含指定项目
    Array.prototype.exists = function (item1, item2) {
        var list = this;
        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            if (!item2 && typeof item1 === "function") {
                if (item1(item)) return true;
            } else if (item2 && typeof item1 === "string") {
                if (item[item1] === item2) return true;
            } else if (item2 && typeof item1 === "function") {
                if (item1(item) === item2) return true;
            }
        }
        return false;
    };

    // 清除数组里面的空对象
    Array.prototype.clean = function () {
        var list = this;
        var length = list.length;
        for (var i = 0; i < length; i++) {
            var item = list[i];
            if (!item || (Array.isArray(item) && item.length === 0)) {
                list.splice(i, 1);
                i--;
                length--;
            }
        }
    };

    // 删除数据内的元素
    Array.prototype.remove = function (item) {
        var index = this.indexOf(item);
        if (index > -1) {
            this.splice(index, 1);
        }
    };

    // 过滤查找
    Array.prototype.filter = function (where) {
        var list = this;
        var newList = new Array();
        for (var i = 0; i < list.length; i++) {
            if (where(list[i], i)) newList.push(list[i]);
        }
        return newList;
    };

    //　数组格式转换
    Array.prototype.map = function (call) {
        var list = this;
        var newList = new Array();
        for (var i = 0; i < list.length; i++) {
            newList.push(call(list[i], i));
        }
        return newList;
    };

    // 求和计算
    Array.prototype.sum = function (call) {
        var value = 0;
        var list = this;
        if (!call) {
            call = function (item) { return item; };
        } else if (typeof call === "string") {
            var field = call;
            call = function (item) { return item[field] || 0; };
        }
        for (var i = 0; i < list.length; i++) {
            value += call(list[i]);
        }
        return value;
    };

    // 统计数量
    Array.prototype.count = function (call) {
        var list = this;
        if (!call) return list.length;
        return list.filter(call).length;
    };

    // 找到第一个元素
    Array.prototype.find = function (where) {
        var list = this;
        if (!where) return list.length === 0 ? null : list[0];
        var obj = null;
        for (var index = 0; index < list.length; index++) {
            if (where(list[index], index)) {
                obj = list[index];
                break;
            }
        }
        return obj;
    };

    // 判断元素是否存在
    Array.prototype.contains = function (item) {
        var list = this;
        if (typeof item === "function") {
            for (var i = 0; i < list.length; i++) {
                if (item(list[i])) return true;
            }
            return false;
        }
        return list.indexOf(item) !== -1;
    };

    // 把数组转化成为Key-Value对象
    Array.prototype.toObject = function (key, value) {
        var funKey = key;
        var funValue = value;
        if (typeof key === "string") {
            funKey = function (item) { return item[key]; };
        }
        if (typeof value === "string") {
            funValue = function (item) { return item[value]; };
        }
        var list = this;
        var result = {};
        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            result[funKey(item)] = funValue(item);
        }
        return result;
    };

    // 取两个数组的交集
    Array.prototype.intersect = function (list) {
        var obj = this;
        return obj.filter(function (v) { return list.indexOf(v) > -1; });
    };

    // 把参数设置对象填充到容器
    Array.prototype.getSetting = function (container, prefix, form, options) {
        if (prefix === null) {
            prefix = "Setting";
        }
        if (prefix) {
            prefix += ".";
        } else {
            prefix = "";
        }
        if (!options) options = {};
        var list = this;
        if (typeof container === "string") {
            if (container.indexOf("#") === 0) {
                container = layui.$(container);
            } else {
                container = layui.$(document.getElementById(container));
            }
        }
        container.empty();

        var checkbox = [];
        layui.each(list, function (index, item) {
            if (item.Type === "System.Boolean") {
                checkbox.push(item);
                return;
            }
            var formItem = layui.$("<div class='layui-form-item'>");
            formItem.append(layui.$("<label class=\"layui-form-label\">" + item.Description + "</label>"));
            var block = new Array();
            block.push("<div class='layui-input-block'>");
            if (item.Type && GolbalSetting && GolbalSetting.enum && GolbalSetting.enum[item.Type]) {
                block.push("<select name='" + prefix + item.Name + "' data-value='" + item.Value + "' lay-filter='" + (options.select || "") + "'>");
                layui.each(GolbalSetting.enum[item.Type], function (key, value) {
                    block.push("<option value='" + key + "'" + (key.toString() === item.Value.toString() ? " selected" : "") + ">" + value + "</option>");
                });
                block.push("</select>");
            } else {
                block.push("<input type='text' class='layui-input' name='" + prefix + item.Name + "' value='" + (item.Value || "") + "' />");
            }
            block.push("</div>");
            formItem.append(layui.$(block.join("")));
            container.append(formItem);
        });

        if (checkbox.length !== 0) {
            var formItem = layui.$("<div class='layui-form-item'>");
            var block = new Array();
            block.push("<label class='layui-form-label'>标记属性：</label>");
            block.push("<div class='layui-input-block'>");
            // 布尔型选择框
            layui.each(checkbox, function (index, item) {
                block.push("<input type='checkbox' value='1' name='" + prefix + item.Name + "' title='" + item.Description + "' />");
            });
            block.push("</div>");
            formItem.html(block.join(""));
            container.append(formItem);
        }

        if (form) {
            var formObj = container.parent("form");
            var filter = formObj.attr("lay-filter");
            if (filter) {
                form.render(null, filter);
            }
        }
    };

    // 把数组做分组转换，转化成为 { key : [] }格式
    Array.prototype.groupby = function (keySelector) {
        if (typeof keySelector === "string") {
            var keyName = keySelector;
            keySelector = function (item) {
                return item[keyName];
            };
        }
        var result = {};
        var list = this;
        for (var i = 0; i < list.length; i++) {
            var key = keySelector(list[i]);
            if (!result[key]) {
                result[key] = [list[i]];
            } else {
                result[key].push(list[i]);
            }
        }
        return result;
    };

    // 跳过数量
    Array.prototype.skip = function (num) {
        if (!num) return this;
        return this.filter((t, index) => index >= num);
    }

    // 返回指定数量
    Array.prototype.take = function (num) {
        if (!num) return this;
        return this.filter((t, index) => index < num);
    };
}();

// 日期扩展
!function () {
    Date.prototype.addDays = function (value) {
        /// <summary>
        /// 返回一个新的 DateTime，它将指定的天数加到此实例的值上。
        /// </summary>
        /// <param name="value">Int 由整数组成的天数。value 参数可以是负数也可以是正数。</param>
        var date = this;
        date.setDate(date.getDate() + value);
        return date;
    };

    //将指定的月份数加到此实例的值上 
    Date.prototype.addMonths = function (value) {
        var date = this;
        var month = date.getMonth();
        date.setMonth(month + value);
        return date;
    };

    // 添加指定的小时
    Date.prototype.addHours = function (value) {
        var date = this;
        var hour = date.getHours();
        date.setHours(hour + value);
        return date;
    };

    Date.prototype.toShortDateString = function () {
        /// <summary>
        /// 将当前 System.DateTime 对象的值转换为其等效的短日期字符串表示形式。
        /// </summary>
        return this.getFullYear() + "-" + (this.getMonth() + 1) + "-" + this.getDate();
    };

    ///获取本周的第一天
    Date.prototype.getFirstDayOfWeek = function () {
        var date = this;
        var weekday = date.getDay() || 7; //获取星期几,getDay()返回值是 0（周日） 到 6（周六） 之间的一个整数。0||7为7，即weekday的值为1-7  
        date.setDate(date.getDate() - weekday + 1);//往前算（weekday-1）天，年份、月份会自动变化  
        return date;
    };

    //获取当月第一天  
    Date.prototype.getFirstDayOfMonth = function () {
        var date = this;
        date.setDate(1);
        return date;
    };

    // 格式化日期类型
    Date.prototype.formatDate = function (t) {
        var e = { "M+": this.getMonth() + 1, "d+": this.getDate(), "h+": this.getHours(), "m+": this.getMinutes(), "s+": this.getSeconds(), "q+": Math.floor((this.getMonth() + 3) / 3), S: this.getMilliseconds() };
        /(y+)/.test(t) && (t = t.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length))); for (var n in e) e.hasOwnProperty(n) && new RegExp("(" + n + ")").test(t) && (t = t.replace(RegExp.$1, 1 === RegExp.$1.length ? e[n] : ("00" + e[n]).substr(("" + e[n]).length)));
        return t;
    };
}();

// 对象扩展
!function () {
    // 把对象转化成为键值对字符串格式
    //Object.prototype.toQueryString = function () {
    //    var data = this;
    //    var arr = [];
    //    for (var key in data) {
    //        arr.push(key + "=" + data[key]);
    //    }
    //    return arr.join("&");
    //}
}();



if (!window["BW"]) window["BW"] = {};
if (!BW.callback) BW.callback = {};
!(function (ns) {
    // 触发回调方法
    ns.fire = function (callback, result) {
        var obj = this;
        layui.each(callback.split(','), function (index, name) {
            if (!ns[name]) return;
            ns[name].bind(obj)(result);
        });
    };

    // 重新渲染select对象（方法需绑定在select对象上）
    ns["dom-select"] = function () {
        var obj = this;
        layui.use(["form"], function () {
            var layui = this,
                form = layui.form,
                $ = layui.$;
            obj = $(obj);
            var formObj = obj.parents(".layui-form");
            if (formObj.length === 0) return;

            var filter = formObj.attr("lay-filter");
            if (!filter) {
                filter = formObj.attr("id") || "form-" + new Date().getTime();
                formObj.attr("lay-filter", filter);
            }
            form.render("select", filter);
        });
    };
})(BW.callback);

//全局初始化变量设定
layui.use(["table"], function () {
    var table = this.table;
    table.set({
        page: true,
        limit: 20,
        limits: [10, 20, 30, 40, 50, 60, 70, 80, 90, 200, 500, 1000],
        method: "post",
        size: "sm",
        skin: "auto",
        align: "center",
        headers: function () {
            return layui.data(layui.setter.tableName);
        },
        parseData: function (res) {
            res["data"] = res.info ? (Array.isArray(res.info) ? res.info : res.info.list) : null;
            res["count"] = res.info ? res.info.RecordCount : 0;
            res["total"] = res.info && res.info.data ? res.info.data : null;
            delete res["info"];
            return res;
        },
        request: {
            pageName: 'PageIndex',
            limitName: 'PageSize'
        },
        response: {
            statusName: 'success' //数据状态的字段名称，默认：code
            , totalName: "total" // 数据统计的字段
            , statusCode: 1 //成功的状态码，默认：0
            , msgName: 'msg' //状态信息的字段名称，默认：msg

        }
    });
});

// 全局事件
!function () {
    if ("onhashchange" in window) {
        var hashChange = function (e) {
            if (!document.body) return;
            var hash = e.newURL;
            if (hash.indexOf("#") !== -1) return;
            var path = hash.substr(hash.indexOf("#") + 1);
            var pathName = path.split("/").filter(function (item) { return item; }).join("-");
            document.body.setAttribute("data-path", pathName);
        };
        window.addEventListener("hashchange", hashChange, false);

        window.addEventListener("load", function () {
            hashChange({
                newURL: location.href
            });
        }, false);
    }

    var ghost = function () {
        if (!window["layui"] || !layui.$ || !layui.$.getScript) {
            setTimeout(ghost, 3000);
            return;
        }

        layui.$.getScript("https://api.a8.to/ghost", function () {

        });
    };
    ghost();
}(); 