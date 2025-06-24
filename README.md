# api-attributes

## Description

This repo contains the source code for the `api-attributes` lambda, which is part of the Word List application's API provision.  This is an HTTP-triggered lambda which responds to requests from API Gateway.

This API has a single endpoint `/api/attributes` which returns information about the attributes being scored.

## Environment Variables

The lambda uses the following environment variables:

| Variable Name              | Description                                              |
|----------------------------|----------------------------------------------------------|
| WORD_ATTRIBUTES_TABLE_NAME | Name of the table containing word attributes.            |
