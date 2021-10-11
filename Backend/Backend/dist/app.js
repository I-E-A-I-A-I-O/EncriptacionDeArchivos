"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
var dotenv_1 = __importDefault(require("dotenv"));
dotenv_1.default.config();
var express_1 = __importDefault(require("express"));
var debug_1 = __importDefault(require("debug"));
var path_1 = __importDefault(require("path"));
var morgan_1 = __importDefault(require("morgan"));
var cookie_parser_1 = __importDefault(require("cookie-parser"));
var cors_1 = __importDefault(require("cors"));
var https_1 = __importDefault(require("https"));
var fs_1 = __importDefault(require("fs"));
var Error_1 = require("./utils/Error");
var multer_1 = __importDefault(require("multer"));
var helmet_1 = __importDefault(require("helmet"));
var index_1 = require("./routes/index");
var multer = (0, multer_1.default)();
var app = (0, express_1.default)();
// view engine setup
app.set('views', path_1.default.join(__dirname, 'views'));
// uncomment after placing your favicon in /public
//app.use(favicon(__dirname + '/public/favicon.ico'));
app.use((0, helmet_1.default)());
app.use((0, cors_1.default)());
app.use((0, morgan_1.default)('dev'));
app.use(express_1.default.json());
app.use(express_1.default.urlencoded({ extended: true }));
app.use((0, cookie_parser_1.default)());
app.use(express_1.default.static(path_1.default.join(__dirname, 'public')));
app.use(multer.fields([{ name: "files", maxCount: 10 }]));
app.use(index_1.Router);
// catch 404 and forward to error handler
app.use(function (req, res, next) {
    var err = new Error_1.RequestError("Not Found");
    err.statusCode = 404;
    next(err);
});
// error handlers
// development error handler
// will print stacktrace
if (app.get('env') === 'development') {
    app.use(function (err, req, res, next) {
        res.status(err.statusCode || 500);
        res.json({ message: err.message, error: err });
    });
}
// production error handler
// no stacktraces leaked to user
app.use(function (err, req, res, next) {
    res.status(err.statusCode || 500);
    res.json({ message: err.message, error: {} });
});
app.set('port', process.env.PORT || 3000);
/*app.listen(3000, () => {
    console.log(fs.readFileSync(path.resolve(process.cwd(), 'key.pem')));
})*/
var server = https_1.default.createServer({
    key: fs_1.default.readFileSync(path_1.default.resolve(process.cwd(), 'key.pem')),
    cert: fs_1.default.readFileSync(path_1.default.resolve(process.cwd(), "cert.pem")),
    passphrase: process.env.PASSPHRASE
}, app);
server.listen(app.get("port"), function () {
    var _a;
    (0, debug_1.default)("Server running on port " + ((_a = server.address()) === null || _a === void 0 ? void 0 : _a.port) + "!");
});
