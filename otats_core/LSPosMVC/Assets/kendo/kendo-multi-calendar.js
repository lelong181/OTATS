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
    var MultiCalendar = /** @class */ (function (_super) {
        __extends(MultiCalendar, _super);
        function MultiCalendar(element, options) {
            var _this = _super.call(this, element, options) || this;
            _this.selectToday = function () {
                var today = new Date();
                var value = new Date(today.getFullYear(), today.getMonth(), today.getDate());
                var values = _this.options.cleanSelectedItemsOnTodayClick
                    ? [value]
                    : _this._values.concat([value]);
                _this.values(values);
            };
            _this._values = (options && options.values) || [];
            _this.navigate();
            return _this;
        }
        MultiCalendar.isSameDate = function (first, second) {
            return (first &&
                second &&
                first.getFullYear() === second.getFullYear() &&
                first.getMonth() === second.getMonth() &&
                first.getDate() === second.getDate());
        };
        MultiCalendar.prototype.values = function (newValues) {
            if (newValues !== undefined && this.canSelectItems(newValues.length)) {
                var valuesToClear = this._values.filter(function (value) {
                    return newValues.every(function (newValue) { return !MultiCalendar.isSameDate(value, newValue); });
                });
                var i_1 = null;
                this._values = newValues.filter(function (item, index) {
                    for (i_1 = index - 1; i_1 >= 0; i_1 -= 1) {
                        if (MultiCalendar.isSameDate(item, newValues[i_1])) {
                            return false;
                        }
                    }
                    return true;
                });
                this.updateSelection(this._values, valuesToClear);
            }
            return this._values;
        };
        MultiCalendar.prototype.value = function (newValue) {
            var valueIndex = (this._values || [])
                .reduce(function (acc, val, index) { return (MultiCalendar.isSameDate(val, newValue) ? index : acc); }, -1);
            var shouldAddNewValue = valueIndex === -1;
            var canAddNewValue = shouldAddNewValue && this._values && this.canSelectItems(this._values.length + 1);
            if (!shouldAddNewValue || canAddNewValue) {
                _super.prototype.value.call(this, newValue);
            }
            if (newValue) {
                var valuesToClear = [];
                if (canAddNewValue) {
                    this._values.push(newValue);
                }
                else if (!shouldAddNewValue) {
                    valuesToClear = this._values.splice(valueIndex, 1);
                }
                this.updateSelection(this._values, valuesToClear);
            }
            return _super.prototype.value.call(this);
        };
        MultiCalendar.prototype.navigate = function (value, view) {
            _super.prototype.navigate.call(this, value, view);
            this.updateSelection(this._values);
        };
        MultiCalendar.prototype.navigateDown = function (value) {
            if (!value) {
                return;
            }
            var index = this._index;
            if (index === MultiCalendar._views[this.options.depth]) {
                this.value(value);
                this.trigger('change');
                return;
            }
            this.navigate(value, (index - 1));
        };
        MultiCalendar.prototype._todayClick = function (e) {
            this.selectToday();
            _super.prototype._todayClick.call(this, e);
        };
        MultiCalendar.prototype.canSelectItems = function (amount) {
            var max = this.options.maxSelectedItems;
            return !max || amount <= max;
        };
        MultiCalendar.prototype.cellByDate = function (value) {
            return this._table.find('td').filter(function () {
                return $(this.firstChild).attr(kendo.attr('value')) === value;
            });
        };
        MultiCalendar.prototype.updateSelection = function (valuesToSelect, valuesToClear) {
            var _this = this;
            if (valuesToClear === void 0) { valuesToClear = []; }
            if (this._index === MultiCalendar._views[this.options.depth]) {
                var selected_1 = 'k-state-selected';
                var _value = this._value;
                if (_value) {
                    this.cellByDate(this.view().toDateString(_value)).removeClass(selected_1);
                }
                valuesToSelect.forEach(function (value) {
                    _this.cellByDate(_this.view().toDateString(value)).addClass(selected_1);
                });
                valuesToClear.forEach(function (value) {
                    _this.cellByDate(_this.view().toDateString(value)).removeClass(selected_1);
                });
            }
        };
        MultiCalendar._views = {
            month: 0,
            year: 1,
            decade: 2,
            century: 3
        };
        return MultiCalendar;
    }(kendo.ui.Calendar));
    kendoExt.MultiCalendar = MultiCalendar;
    MultiCalendar.fn = MultiCalendar.prototype;
    MultiCalendar.fn.options = __assign({}, kendo.ui.Calendar.fn.options, {
        name: 'MultiCalendar',
        values: [],
        maxSelectedItems: null,
        cleanSelectedItemsOnTodayClick: true
    });
    kendo.ui.plugin(MultiCalendar);
})(kendoExt || (kendoExt = {}));
//# sourceMappingURL=kendo-multi-calendar.js.map