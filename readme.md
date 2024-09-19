# Cloud Databases assignment

## Installation

For some reason adding a table through Azure functions doesn't work.

`Emulator & Attached > Storage Accounts > (Emulator - Default Ports) (Key) > Tables`

In the repo above, make a table called `orders`.

## Usage

Running `PlaceOrder` runs all projects.

1. call `/order/place` with the following json body:
```json
{
	"Product": 1,
	"Customer": "Billy Bob",
	"Address": "Sesame Street 123"
}
```
2. call `/order/ship` with a plain text body containing the queue's message id, which is posted in the log.

example: `ed27bef9-5aea-4885-ad24-ce6a988a9acf`

## Assignment

This Repo contains the (work in progress) of the following assignment:

Widget and Co – product widget - have decided to transfer their website from a local ISP hosting company to the cloud. The site has become very popular during the past few years and can experience heavy loads at peak hours. It is suggested that the move to Azure could alleviate some of the occurring problems.

During peak hours a huge amount of traffic is generated by online users who place their orders. In the past the order placing process has been the main culprit in the sluggish performance of the website. 

The orderdate is stored; after an order is shipped the shipping date is again stored – these are used to calculate orderprocessed metric. 

As Widget and Co is part of the conglomerate Wiley and Co the user info is shared with another department.

The product specification is kept in a traditional SQL database, Images of the product are also stored. 

The site also runs a forum part where users can post reviews of the products (anonymously) – at a later stage the marketing department would use the reviews to profile a new range of products. 

_**Your task:**_

The Online web store will be hosted (and run) in a cloud environment (Azure). Design a proof of concept (in C# Web API with at least 2 Azure Functions) of the application where special attention is paid to the design of the Cloud Database architecture. 

The proof of concept does not require a front end!