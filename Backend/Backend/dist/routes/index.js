"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.Router = void 0;
var express_1 = __importDefault(require("express"));
var index_1 = require("../controllers/index");
exports.Router = express_1.default.Router({
    strict: true
});
exports.Router.get("/hello", index_1.SayHello);
exports.Router.post("/file", index_1.ReceiveFile);
