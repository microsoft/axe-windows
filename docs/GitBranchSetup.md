<!--
Copyright (c) Microsoft Corporation. All rights reserved.
Licensed under the MIT License.
-->

## Git branch setup

This document describes how to set up your development environment and contribute changes to
[axe-windows](https://github.com/Microsoft/axe-windows). This document assumes basic working knowledge
of Git and related tools. The instructions are specific to this project.

## Creating your own fork

If you wish to contribute changes back to the [axe-windows](https://github.com/Microsoft/axe-windows)
repository, start by creating your own [Fork](https://help.github.com/en/articles/fork-a-repo) of the repository. This will keep down the number of branches on the main repository. In your own fork, you can create as many branches as you like.

-   Navigate to [GitHub](https://github.com/) with a browser and log in to your GitHub account. For the sake of this document, let's assume your username is **ada-cat**.
-   Navigate to the [axe-windows](https://github.com/Microsoft/axe-windows) repository in the same browser session.
-   Click on the **Fork** button at the top right corner of the page.
-   Create the fork under your account. Your GitHub profile should now show **axe-windows** as one of your repositories.
-   Create a folder on your device and clone your fork of the **axe-windows** repository. e.g. `https://github.com/ada-cat/axe-windows`. Notice how your GitHub username is in the repository location.

```bash
git clone https://github.com/ada-cat/axe-windows
```

## Setting up the upstream repository

Before starting to contribute changes, please setup your upstream repository to the
primary **axe-windows** repository.

-   When you run git remote -v, you should see only your fork in the output list

```bash
git remote -v
origin  https://github.com/ada-cat/axe-windows (fetch)
origin  https://github.com/ada-cat/axe-windows (push)
```

-   Map the primary **axe-windows** as the upstream remote

```bash
git remote add upstream https://github.com/Microsoft/axe-windows
```

-   Now, running `git remote -v` should show the upstream repository also

```bash
git remote -v
origin  https://github.com/ada-cat/axe-windows (fetch)
origin  https://github.com/ada-cat/axe-windows (push)
upstream        https://github.com/Microsoft/axe-windows (fetch)
upstream        https://github.com/Microsoft/axe-windows (push)
```

-   At this point you are ready to start branching and contributing back changes.

## Default branch

As of February 2021, the default branch is the `main` branch

## Making code changes and creating a pull request

Create a branch from your fork and start making the code changes. Once you are happy with the changes, and want to merge them to the main **axe-windows** project, create a pull request from your branch directly to "Microsoft/axe-windows main".

## Merging upstream main into your fork main

From time to time, your fork will get out of sync with the upstream remote. Use the following commands to get the main branch of your fork up to date.

```bash
git fetch upstream
git checkout main
git pull upstream main
git push
```

## Merging upstream main into your current branch

Use these commands instead if you would like to update your current branch in your fork from the upstream remote.

```bash
git fetch upstream
git pull upstream main
git push
```
 