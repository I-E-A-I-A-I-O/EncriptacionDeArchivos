import { Request, Response, NextFunction } from "express";

export const SayHello = (req: Request, res: Response, next: NextFunction) => {
    res.send("Hello world!");
};

export const ReceiveFile = (req: Request, res: Response, next: NextFunction) => {
    res.sendStatus(501);
};
