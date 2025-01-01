# Security Policy

This is our policy for reporting security vulnerabilities and overall guidelines on what you should do upon discovering one.

> [!NOTE]
> This document also outlines the measures we have put in to prevent security vulnerabilities in the first place.

---

<!--
### Supported Versions

Use this section to tell people about which versions of your project are
currently being supported with security updates.

| Version | Supported          |
| ------- | ------------------ |
| 5.1.x   | :white_check_mark: |
| 5.0.x   | :x:                |
| 4.0.x   | :white_check_mark: |
| < 4.0   | :x:                |
-->

## Reporting Security Vulnerabilities

<!--
Use this section to tell people how to report a vulnerability.

Tell them where to go, how often they can expect to get an update on a
reported vulnerability, what to expect if the vulnerability is accepted or
declined, etc.
-->

**Please report security advistiry from "New draft security advisory" in the Security tab**

In order to report a security vulnerability, you can use [GitHub's built-in tool](https://github.com/FireCubeStudios/DarkSky/security/advisories/new) which easily allows you to calculate an _attack vector/CVSS string_ or attribute to an existing [`CVE`](https://cve.org) code. This allows us to accurately calculate the severity and/or importance of preventing it.

### Spotting secrets in code

If you spot a secret in the code, please let us know by contacting us on Discord via private DM. This helps us quietly remove the vulnerability without letting others abuse it.

### GitHub API

DarkSky relies heavily on the BlueSky (ATProtocol) API via open source project FishyFlip. If you believe that you have found a security vulnerability in the API and not DarkSky, _please please please_ email BlueSky via `security@bsky.app`, BlueSky's official vulnerability report email.
> If you believe you have found a security vulnerability, please email us at `security@bsky.app` with a description of the issue.

## Our Measures

What have we done to keep DarkSky safe?

### Dependabot

We have implemented Dependabot alerts to automatically track security vulnerabilities that apply to the repository's dependencies.

### Code scanning

We have enabled GitHub Code Scanning to automatically scan our code for potential GitHub client secrets and other API tokens.

### Security advisories

We have enabled GitHub security advisories to let us know if a potential security problem might affect our repository or if something doesn't look right with any of our other security vulnerability countermeasures. This makes it easy to track potential errors or problems that might expose user credentials publicly or cause other similar problems.
