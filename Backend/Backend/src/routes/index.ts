import express, { Request, Response } from "express";
import { ReceiveFile, SayHello } from "../controllers/index";

export const Router = express.Router({
    strict: true
});

Router.get("/hello", SayHello);
Router.post("/file", ReceiveFile);
