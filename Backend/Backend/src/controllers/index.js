"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ReceiveFile = exports.SayHello = void 0;
var SayHello = function (req, res, next) {
    res.send("Hello world!");
};
exports.SayHello = SayHello;
var ReceiveFile = function (req, res, next) {
    res.sendStatus(501);
};
exports.ReceiveFile = ReceiveFile;
//# sourceMappingURL=index.js.map