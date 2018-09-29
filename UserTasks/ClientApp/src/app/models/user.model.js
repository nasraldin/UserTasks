"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var User = /** @class */ (function () {
    function User(id, userName, fullName, email, roles) {
        this.id = id;
        this.userName = userName;
        this.fullName = fullName;
        this.email = email;
        this.roles = roles;
    }
    Object.defineProperty(User.prototype, "friendlyName", {
        get: function () {
            var name = this.fullName || this.userName;
            return name;
        },
        enumerable: true,
        configurable: true
    });
    return User;
}());
exports.User = User;
//# sourceMappingURL=user.model.js.map