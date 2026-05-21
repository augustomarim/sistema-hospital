# 🏥 Sistema de Gestão Hospitalar

Sistema desktop desenvolvido em C# com WPF (.NET 10), seguindo a arquitetura MVC, princípios SOLID e Clean Code, com persistência em banco de dados SQLite via Entity Framework Core 10.

---

## 🛠️ Tecnologias Utilizadas

- **C# / .NET 10**
- **WPF** (Windows Presentation Foundation)
- **Entity Framework Core 10** (Code First)
- **SQLite**
- **SQL puro** (ADO.NET) — busca de consultas por período
- **Microsoft.Extensions.DependencyInjection**

---

## 🏗️ Arquitetura

O projeto segue a arquitetura **MVC (Model, View, Controller)**:

| Camada | Pasta | Responsabilidade |
|---|---|---|
| **Model** | `Models/` | Entidades e regras de negócio |
| **View** | `Views/` | Telas XAML |
| **Controller** | `Controllers/` | Ponte entre View e Model |
| **Data** | `Data/` | DbContext e Repositórios |

---

## 📐 Princípios SOLID Aplicados

| Princípio | Onde foi aplicado |
|---|---|
| **SRP** — Single Responsibility | Cada Controller gerencia apenas uma entidade |
| **OCP** — Open/Closed | `Funcionario` é estendido por `Medico` e `Enfermeiro` sem modificação |
| **LSP** — Liskov Substitution | `Medico` e `Enfermeiro` substituem `Funcionario` sem quebrar o contrato |
| **ISP** — Interface Segregation | `IEspecialidade` é pequena e focada, `IRepository<T>` é genérica e coesa |
| **DIP** — Dependency Inversion | Controllers recebem `IRepository<T>`, nunca a implementação concreta |

---

## 🧹 Clean Code Aplicado

| Prática | Onde foi aplicado |
|---|---|
| Nomes descritivos | `CarregarTodosAsync`, `ValidarPaciente`, `LimparFormulario` |
| Métodos pequenos com uma responsabilidade | Métodos privados de validação nos Controllers |
| Sem comentários desnecessários | Código se explica pelos nomes |
| Tratamento de exceção claro | `ArgumentException` com mensagens claras nos Controllers |

---

## 🗄️ Banco de Dados — Relacionamentos

| Tipo | Entidades |
|---|---|
| **1x1** | Paciente → FichaMedica |
| **1xN** | Departamento → Funcionários |
| **1xN** | Paciente → Consultas |
| **1xN** | Médico → Consultas |
| **NxN** | Paciente ↔ Médico via Consulta |
| **Herança (TPH)** | Pessoa → Funcionário → Médico / Enfermeiro |

---

## ✅ Funcionalidades

- Cadastro de Pacientes com Ficha Médica automática
- Cadastro de Médicos e Enfermeiros
- Cadastro de Departamentos
- Agendamento de Consultas
- Busca de Consultas por período via **SQL puro**
- Validação de CPF pelo algoritmo oficial do governo
- Máscaras de CPF e Telefone
- Validação de duplicatas (CPF, CRM, COREN)

---

## 🤖 Uso de IA — AI Assessment Scale (IA-2)

O desenvolvimento utilizou IA dentro do nível **IA-2** da AI Assessment Scale:

- IA utilizada para **exploração e planejamento** da arquitetura
- Todo código gerado passou por **revisão humana completa**
- IA não substituiu o raciocínio crítico do desenvolvedor

### Ferramentas utilizadas
- **Claude (Anthropic)** — geração inicial de estrutura e código com revisão humana

---

## ▶️ Como Executar

1. Clone o repositório
2. Abra o `HospitalSystem.sln` no Visual Studio
3. Restaure os pacotes NuGet
4. Pressione **F5** para rodar

> O banco de dados é criado automaticamente na primeira execução via `EnsureCreated()`.

---
