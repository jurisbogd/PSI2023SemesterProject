# PSI2023SemesterProject

* [Core idea](#core-idea)
* [Flashcards and flashcard packs](#flashcards-and-flashcard-packs)
* [Flashcard creation and editing](#flashcard-creation-and-editing)
* [User created flashcard pack sharing](#user-created-flashcard-pack-sharing)
* [Spaced repetition](#spaced-repetition)

## Core idea
A web service that enables users to easily create and share flashcards with some sort of spaced repetition functionality.

## Flashcards and flashcard packs
Flashcards are used as a memorization tool, usually following the format of a question on one side of the card and an answer on the other.

Our web service will be able to load and display groups of flashcards (hereafter known as packs) as well as allow users to show/hide the answer and select whether they answered correctly or incorrectly, allowing the user to keep track of score and gauge improvement more effectively.

## Flashcard creation and editing
Users will be able to create and publish their own flashcard packs.

We might add support for mathematical symbols, code snippets, special formatting and images in flashcards.

## User created flashcard pack sharing
Flashcard packs will be shareable with a single web link.

Users may be able to grant other users permission to edit flashcard packs, to allow groups of people to work on one pack.

## Spaced repetition
We plan to allow users to subscribe to flashcard packs for the purpose of [spaced repetition](https://en.wikipedia.org/wiki/Spaced_repetition).

Instead of selecting a specific pack, a user will be able to do a sort of 'general repetition' which will include all the packs that they are subscribed to.

During the general repetition, the spaced repetition system will show the user several new cards per some time interval (amount of new cards and length of the time interval could be editable). Occasionally the system will show some older cards as well. Older cards will be shown more frequently if the user struggles with them, and likewise will be shown less frequently if the user is able to recall correctly, taking advantage of the [spacing effect](https://en.wikipedia.org/wiki/Spacing_effect).
