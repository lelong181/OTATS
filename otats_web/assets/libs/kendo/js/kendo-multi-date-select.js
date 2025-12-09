var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};
var kendoExt;
(function (kendoExt) {
    var MultiDateSelect = /** @class */ (function (_super) {
        __extends(MultiDateSelect, _super);
        function MultiDateSelect(element, options) {
            var _this = _super.call(this, element, options) || this;
            _this.initMultiSelect(element);
            _this.initPopup(element);
            _this.initCalendar(_this._popup.element);
            _this.updateDateInterval();
            return _this;
        }
        MultiDateSelect.removeTime = function (date) {
            var d = new Date(date.toString());
            d.setMilliseconds(0);
            d.setSeconds(0);
            d.setMinutes(0);
            d.setHours(0);
            return d;
        };
        MultiDateSelect.isDateGreater = function (first, second) {
            return (MultiDateSelect.removeTime(first).getTime() > MultiDateSelect.removeTime(second).getTime());
        };
        MultiDateSelect.isDateLesser = function (first, second) {
            return (MultiDateSelect.removeTime(first).getTime() < MultiDateSelect.removeTime(second).getTime());
        };
        MultiDateSelect.prototype.open = function () {
            this._popup.open();
        };
        MultiDateSelect.prototype.close = function () {
            this._popup.close();
        };
        MultiDateSelect.prototype.toggle = function () {
            if (this._popup.visible()) {
                this.close();
            }
            else {
                this.open();
            }
        };
        MultiDateSelect.prototype.destroy = function () {
            this._multiSelect.wrapper.find('input').off('keydown');
            this._popup.destroy();
            this._multiSelect.destroy();
            this._multiCalendar.destroy();
            _super.prototype.destroy.call(this);
        };
        MultiDateSelect.prototype.enable = function (enable) {
            if (!enable) {
                this.close();
            }
            this._multiSelect.enable(enable);
        };
        MultiDateSelect.prototype.readonly = function (readonly) {
            if (readonly) {
                this.close();
            }
            this._multiSelect.readonly(readonly);
        };
        MultiDateSelect.prototype.max = function (max) {
            if (max !== undefined) {
                this._multiCalendar.max(max);
                this.updateDateInterval();
            }
            return this._multiCalendar.max();
        };
        MultiDateSelect.prototype.min = function (min) {
            if (min !== undefined) {
                this._multiCalendar.min(min);
                this.updateDateInterval();
            }
            return this._multiCalendar.min();
        };
        MultiDateSelect.prototype.value = function (values) {
            return this.values(values);
        };
        MultiDateSelect.prototype.values = function (values) {
            if (values !== undefined) {
                var min_1 = this.min();
                var max_1 = this.max();
                var vs = values
                    .filter(function (date) {
                    return !((min_1 && MultiDateSelect.isDateLesser(date, min_1)) ||
                        (max_1 && MultiDateSelect.isDateGreater(date, max_1)));
                });
                this._multiCalendar.values(vs);
                this.updateMultiSelectValues(vs);
                if (vs.length) {
                    this._multiCalendar.navigate(vs[vs.length - 1]);
                }
            }
            return this._multiSelect.value();
        };
        MultiDateSelect.prototype.multiSelect = function () {
            return this._multiSelect;
        };
        MultiDateSelect.prototype.multiCalendar = function () {
            return this._multiCalendar;
        };
        MultiDateSelect.prototype.initMultiSelect = function (parent) {
            var _this = this;
            var options = this.options;
            var defaultTemplate = (function (data) { return kendo.toString(data, options.format); });
            options.tagTemplate = options.tagTemplate || defaultTemplate;
            var open = function (e) {
                e.preventDefault();
                _this.open();
            };
            var change = function () {
                _this._multiCalendar.values(_this._multiSelect.value());
                _this._popup.position();
                _this.trigger(MultiDateSelect.changeEvent);
            };
            this._multiSelect = $('<select multiple></select>')
                .appendTo(parent)
                .kendoMultiSelect({
                dataSource: options.values,
                value: options.values,
                ignoreCase: false,
                enable: options.enable,
                maxSelectedItems: options.maxSelectedItems,
                placeholder: options.placeholder,
                tagTemplate: options.tagTemplate,
                open: open,
                change: change
            })
                .data('kendoMultiSelect');
            this._multiSelect._filterSource = function () { return ({}); };
            this._multiSelect.search = function () { return ({}); };
            this._multiSelect.wrapper.find('input').on('keydown', function (e) {
                var key = e.keyCode;
                var inputValue = e.target.value;
                if (key === kendo.keys.ENTER) {
                    var parsedDate = kendo.parseDate(inputValue, options.format);
                    var values = _this.values();
                    if (parsedDate && parsedDate <= _this.max() && parsedDate >= _this.min()) {
                        var dates = values.concat(parsedDate);
                        _this._multiCalendar.values(dates);
                        _this.updateMultiSelect();
                        _this._multiCalendar.navigate(parsedDate);
                        _this.trigger(MultiDateSelect.changeEvent);
                    }
                }
            });
        };
        MultiDateSelect.prototype.initPopup = function (parent) {
            var _this = this;
            var open = function () { return _this.trigger(MultiDateSelect.openEvent); };
            var close = function () { return _this.trigger(MultiDateSelect.closeEvent); };
            this._popup = $('<div class="k-calendar-container"></div>')
                .appendTo(document.body)
                .kendoPopup(__assign({}, this.options.popup, {
                name: 'Popup',
                animation: this.options.animation,
                anchor: parent,
                open: open,
                close: close
            }))
                .data('kendoPopup');
        };
        MultiDateSelect.prototype.initCalendar = function (parent) {
            var _this = this;
            var options = this.options;
            var change = function () {
                _this.updateMultiSelect();
                _this.trigger(MultiDateSelect.changeEvent);
            };
            var navigate = function () { return _this.trigger(MultiDateSelect.navigateEvent); };
            this._multiCalendar = $('<div></div>')
                .appendTo(parent)
                .kendoMultiCalendar({
                values: options.values,
                footer: options.footer,
                culture: options.culture,
                min: options.min,
                max: options.max,
                start: options.start,
                depth: options.depth,
                month: options.month,
                dates: options.dates,
                maxSelectedItems: options.maxSelectedItems,
                cleanSelectedItemsOnTodayClick: options.cleanSelectedItemsOnTodayClick,
                change: change,
                navigate: navigate
            })
                .data('kendoMultiCalendar');
            kendo.calendar.makeUnselectable(this._multiCalendar.element);
        };
        MultiDateSelect.prototype.updateDateInterval = function () {
            this.values(this.values());
        };
        MultiDateSelect.prototype.updateMultiSelectValues = function (values) {
            this._multiSelect.setDataSource(values);
            this._multiSelect.value(values);
            if (this._popup.visible()) {
                this._popup.position();
            }
        };
        MultiDateSelect.prototype.updateMultiSelect = function () {
            this.updateMultiSelectValues(this._multiCalendar.values());
            if (this.options.autoClose) {
                this.close();
            }
        };
        MultiDateSelect.navigateEvent = 'navigate';
        MultiDateSelect.changeEvent = 'change';
        MultiDateSelect.openEvent = 'open';
        MultiDateSelect.closeEvent = 'close';
        return MultiDateSelect;
    }(kendo.ui.Widget));
    kendoExt.MultiDateSelect = MultiDateSelect;
    MultiDateSelect.fn = MultiDateSelect.prototype;
    MultiDateSelect.fn.options = __assign({}, kendo.ui.Widget.fn.options, {
        name: 'MultiDateSelect',
        autoClose: true,
        popup: {},
        animation: {},
        enable: true,
        maxSelectedItems: null,
        cleanSelectedItemsOnTodayClick: true,
        placeholder: '',
        tagTemplate: '',
        values: null,
        footer: '',
        culture: '',
        format: 'M/d/yyyy',
        min: new Date(1900, 0, 1),
        max: new Date(2099, 11, 31),
        start: 'month',
        depth: 'month',
        month: {},
        dates: []
    });
    MultiDateSelect.fn.events = [
        MultiDateSelect.navigateEvent,
        MultiDateSelect.changeEvent,
        MultiDateSelect.openEvent,
        MultiDateSelect.closeEvent
    ];
    kendo.ui.plugin(MultiDateSelect);
})(kendoExt || (kendoExt = {}));
//# sourceMappingURL=kendo-multi-date-select.js.map