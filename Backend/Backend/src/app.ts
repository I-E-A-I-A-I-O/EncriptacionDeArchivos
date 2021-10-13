import dotenv from "dotenv"
dotenv.config();
import express, { Request, Response, NextFunction } from "express";
import debug from "debug";
import path from "path";
import logger from "morgan";
import cookieParser from "cookie-parser";
import { AddressInfo } from "net";
import cors from "cors";
import https from "https";
import fs from "fs";
import { RequestError } from "./utils/Error";
import Multer from "multer";
import helmet from "helmet";
import { Router as indexRouter } from "./routes/index";

const multer = Multer();
const app = express();

// view engine setup
app.set('views', path.join(__dirname, 'views'));

// uncomment after placing your favicon in /public
//app.use(favicon(__dirname + '/public/favicon.ico'));
app.use(helmet());
app.use(cors());
app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));
app.use(multer.fields([{ name: "files" }]));

app.use(indexRouter);

// catch 404 and forward to error handler
app.use(function (req: Request, res: Response, next: NextFunction) {
    var err = new RequestError("Not Found");
    err.statusCode = 404;
    next(err);
});

// error handlers

// development error handler
// will print stacktrace
if (app.get('env') === 'development') {
    app.use(function (err: RequestError, req: Request, res: Response, next: NextFunction) {
        res.status(err.statusCode || 500);
        res.json({ message: err.message, error: err });
    });
}

// production error handler
// no stacktraces leaked to user
app.use(function (err: RequestError, req: Request, res: Response, next: NextFunction) {
    res.status(err.statusCode || 500);
    res.json({ message: err.message, error: {} });
});

app.set('port', process.env.PORT || 3000);

/*app.listen(3000, () => {
    console.log(fs.readFileSync(path.resolve(process.cwd(), 'key.pem')));
})*/

var server = https.createServer({
    key: fs.readFileSync(path.resolve(process.cwd(), 'key.pem')),
    cert: fs.readFileSync(path.resolve(process.cwd(), "cert.pem")),
    passphrase: process.env.PASSPHRASE
}, app);

server.listen(app.get("port"), function () {
    debug(`Server running on port ${(server.address() as AddressInfo)?.port}!`);
});
