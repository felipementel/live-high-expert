---
# Trigger - when should this workflow run?
# on:
#   workflow_dispatch:  # Manual trigger

# Alternative triggers (uncomment to use):
# on:
#   issues:
#     types: [opened, reopened]
#   pull_request:
#     types: [opened, synchronize]
#   schedule: daily  # Fuzzy daily schedule (scattered execution time)
#   # schedule: weekly on monday  # Fuzzy weekly schedule

# Permissions - what can this workflow access?
# Write operations (creating issues, PRs, comments, etc.) are handled
# automatically by the safe-outputs job with its own scoped permissions.
# permissions:
#   contents: read
#   issues: read
#   pull-requests: read

# Tools - GitHub API access via toolsets (context, repos, issues, pull_requests)
# tools:
#   github:
#     toolsets: [default]

# Network access
# network: defaults

# Outputs - what APIs and tools can the AI use?
# safe-outputs:
#   create-issue:          # Creates issues (default max: 1)
#     max: 5               # Optional: specify maximum number
  # actions:
  # activation-comments:
  # add-comment:
  # add-labels:
  # add-reviewer:
  # allowed-github-references:
  # assign-milestone:
  # assign-to-agent:
  # assign-to-user:
  # autofix-code-scanning-alert:
  # call-workflow:
  # close-discussion:
  # close-issue:
  # close-pull-request:
  # concurrency-group:
  # create-agent-session:
  # create-agent-task:
  # create-check-run:
  # create-code-scanning-alert:
  # create-discussion:
  # create-project:
  # create-project-status-update:
  # create-pull-request:
  # create-pull-request-review-comment:
  # dismiss-pull-request-review:
  # dismiss-review:
  # dispatch-repository:
  # dispatch-workflow:
  # dispatch_repository:
  # environment:
  # failure-issue-repo:
  # footer:
  # group-reports:
  # hide-comment:
  # id-token:
  # link-sub-issue:
  # mark-pull-request-as-ready-for-review:
  # max-bot-mentions:
  # max-patch-files:
  # mentions:
  # merge-pull-request:
  # missing-data:
  # missing-tool:
  # noop:
  # push-to-pull-request-branch:
  # remove-labels:
  # replace-label:
  # reply-to-pull-request-review-comment:
  # report-failure-as-issue:
  # report-incomplete:
  # resolve-pull-request-review-thread:
  # scripts:
  # set-issue-field:
  # set-issue-type:
  # steps:
  # submit-pull-request-review:
  # threat-detection:
  # unassign-from-user:
  # update-discussion:
  # update-issue:
  # update-project:
  # update-pull-request:
  # update-release:
  # upload-artifact:
  # upload-asset:
  # urls:

name: Open Pull Request Handler
on:
  pull_request:
    types: [opened]
permissions:
  contents: read
  issues: read
  pull-requests: read
  copilot-requests: write #https://github.blog/changelog/2026-06-11-agentic-workflows-no-longer-need-a-personal-access-token/

# Tools - GitHub API access via toolsets (context, repos, issues, pull_requests)
tools:
  github:
    toolsets: [default]

# Network access
network: defaults

# Outputs - what APIs and tools can the AI use?
safe-outputs:
  create-issue:          # Creates issues (default max: 1)
    max: 5               # Optional: specify maximum number
  mentions:             # Mentions users in comments or issues
    max: 5               # Optional: specify maximum number
  add-comment:          # Adds comments to issues or PRs
    max: 5               # Optional: specify maximum number
  add-labels:           # Adds labels to issues or PRs
    max: 5               # Optional: specify maximum number
  assign-to-user:
---

# pull-request-opened

Faça ao code review do pull request aberto, fornecendo feedback e sugestões de melhorias. Analise o código, identifique problemas potenciais, e sugira alterações ou melhorias. Certifique-se de que o código segue as melhores práticas e padrões do projeto.

## Instructions

Forneça um feedback detalhado sobre o pull request, incluindo:
- Pontos fortes do código.
- Áreas que podem ser melhoradas.
- Sugestões de refatoração ou otimização.

IMPORTANTE: Todas as novas variaveis precisam começar com o prefixo "fe_" para evitar conflitos com variáveis existentes. Por exemplo, use `fe_newVariable` em vez de `newVariable`.


Ao final, escreva um poema sobre a vida do desenvolvedor de software, destacando os desafios e as recompensas da profissão.
