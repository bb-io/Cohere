# Blackbird.io Cohere

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

Cohere is an artificial intelligence company that specializes on large language models (LLMs) trained on massive amounts of text data. These LLMs help businesses build software that can search, understand, generate and summarize text.

## Before setting up

Before you can connect you need to make sure that:

- You have a Cohere account.
- You have an API key for your Cohere account. Learn more about Cohere's API keys [here](https://docs.cohere.com/reference/create-an-api-key%20).

## Connecting

1. Navigate to apps and search for Cohere. If you cannot find Cohere then click _Add App_ in the top right corner, select Cohere and add the app to your Blackbird environment.
2. Click _Add Connection_.
3. Name your connection for future reference e.g. 'My Cohere connection'.
4. Fill in the API key for your Cohere account.
7. Click _Connect_.

![CohereBlackbirdConnection](image/README/CohereBlackbirdConnection.png)

## Chat

- **Analyze text** Analyze text to retrieve information about its style, mood and tone.
- **Calculate similarity of two texts** Calculate the similarity of texts provided. The result of this action is a percentage similarity score. The higher the score, the more similar the texts are.
- **Detect locale** Detect locale of the text provided.
- **Rewrite text** Rewrite the input text given an instruction prompt (e.g. make it more concise or make it more friendly). This action was called "Edit text" before; existing Birds keep working.
- **Extract entity from text** Extract a piece of information from text. Provide entity that you want to extract from a text (e.g. product title).
- **Generate embedding** Generate text embedding. An embedding is a list of floating point numbers that captures semantic information about the text that it represents.
- **Generate text** Generate realistic text conditioned on a given input.
- **Perform grammar and spelling check** Perform a grammar and spelling check of the text provided.
- **Rerank texts** This action takes in a query and a list of texts and produces an ordered list with each text assigned a relevance score.
- **Rerank texts provided in a file** This action takes in a query and a txt file with list of texts and produces a text combined from most relevant texts. Each text in the file must start on a new line.
- **Reshape text** Reshape the text. Provide the information about target style, mood and tone.
- **Summarise text analyses** Summarise information about styles, moods and tones of different texts to find common patterns in styles, moods and tones.
- **Tokenize text** Tokenize text. Specify model to ensure that the tokenization uses the tokenizer used by specific model.

### Translation

- **Translate** Translate file content retrieved from a CMS or file storage. Only segments without a translation are translated. Supports a glossary, additional instructions, tone and an optional document summary.
- **Translate text** Translate text from one language to another. The source language is detected when it is not set.

### Editing

- **Edit** Edit a translated file. Translated segments are post-edited and set to reviewed.
- **Edit text** Review translated text and return an edited version.

### Review

- **Review** Score each translated segment of a file from 0 to 1. Segments at or above the score threshold (default 0.8) are set to final.
- **Review text** Score the quality of a translated text from 0 to 1.

### Shortening

- **Shorten content** Shorten translated file content to fit the size restrictions of its units or the maximum length.
- **Shorten text** Shorten text to fit the maximum length.

### Summarizing

- **Summarize** Summarize file content for a target audience and language. Translated content is summarized from its translation.
- **Summarize text** Summarize text for a target audience and language.

All actions above run on the Blackbird AI engine: requests are split into token-aware batches, failed or truncated answers are retried, and warnings are written to the flight log. When the default model is not changed, translation actions use `command-a-translate-08-2025` and the other actions use Cohere's default chat model. Requests that hit the Cohere rate limit are retried automatically.

## Feedback

Do you want to use this app or do you have feedback on our implementation? Reach out to us using the [established channels](https://www.blackbird.io/) or create an issue.

<!-- end docs -->
