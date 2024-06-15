## Thought process

After reviewing the document, I kept thinking about the rationale behind the constraints (ORMs and libraries), I interpreted that the essence of the challenge lies in crafting a modern API without relying on ton modern library comforts, I've tried that approach whenever I could.

Initially, I was inclined to employ a NoSQL/key-value store to simplify storage, but I ended up going with Postgres to keep it from being too straightforward.

Then there was the whole dilemma about which domain to pick for the API (not authentication-related). Drawing on my background in aviation, I selected it as the primary domain. After that, it was a puzzle figuring out how to set up the database tables without migrations, I settled on using SQL scripts that run during the Postgres container initialization.

Once I had all these blockers resolved, I focused on structuring projects and directories, preparing to embark on code development from the domain perspective. While striving to apply Domain-Driven Design (DDD), the modest scale of the domain somewhat tempered its visible application.

Progressing through the layers of hexagonal architecture, I culminated in the completion of the APIs, At this stage, I enhanced the Swagger documentation to facilitate testing directly from that platform,  skipping the whole idea of building a few Angular interfaces.

For the authentication API, I was very tempted to use IdentityServer, but sticking to the challenge's idea, I decided to implement something fairly simple that generates JWT tokens. However, this simplicity involves security shortcuts that should not be implemented in production systems.

## User Story

As an airline, I want to manage the weight of my aircraft fleet by model through an API, I need to create, edit, and delete airplanes, and require basic authentication for this API. Additionally, I should be able to create user accounts through this authentication system.