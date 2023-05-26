Finance API

WebAPI - .Net Core 6.0 - EntityFramework Core 6 (code first)
Oracle Autonomous Database

Hosting strategy:
Oracle Cloud: Autonomous Database
AWS: App Runner (publish a container)

Continuous Deployment Strategy:

Artifacts:
1) jpancher/FinanceApi6 public repository in GitHub
2) Oracle Cloud Infrastructure account
2.1) Transactional Oracle Autonomous Database (up to 20GB free forever)
3) ASW account
3.1) ECR
3.2) App Runner

Run the Entity Framework commands locally, accessing the Autonomous Database in Oracle Cloud Infrastructure
	- migrations
	- update-database

Build the container image (the oracle wallet files is copied into the image)
Tag the container image
Push the container image to AWS ECR
Create a new App Runner using the pushed container image

After first deploy, to trigger an update, the ECR image have to be updated.




