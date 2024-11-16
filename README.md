# Camus


"**Camus**" is an utility library for Unity3d. This library can be download from **GitHub**. Some code is collect from Unity Forum and GitHub wih a reference link.


## Principle

- Commit Rule
    - Keep commit “ATOMIC”
        - [DEVELOPER TIP: KEEP YOUR COMMITS “ATOMIC”](https://www.freshconsulting.com/atomic-commits/)
        - Rules
            1. Commit each fix or task as a separate change
            2. Only commit when a block of work is complete
            3. Commit each layout change separately
            4. Joint commit for layout file, code behind file, and additional resources
    - Write good commit message
        - [How to Write a Git Commit Message](https://chris.beams.io/posts/git-commit/)
        - Rules
            1. Separate subject from body with a blank line
            2. Limit the subject line to 50 characters
            3. Capitalize the subject line
            4. Do not end the subject line with a period
            5. Use the imperative mood in the subject line
            6. Wrap the body at 72 characters
            7. Use the body to explain what and why vs. how
- Architecture Decision Record (ADR)
    - [ADR](https://adr.github.io/)
    - Use The Markdown Architecture Decision Records (MADR) template to create ADR
- Version
    - [Semantic Versioning 2.0.0](https://semver.org/)
    - Given a version number MAJOR.MINOR.PATCH, increment the:
        - **MAJOR** version when you make incompatible API changes,
        - **MINOR** version when you add functionality in a backwards-compatible manner, and
        - **PATCH** version when you make backwards-compatible bug fixes.


## Folders

### Global

- Files in **Global** folder will not add to any namespace.

