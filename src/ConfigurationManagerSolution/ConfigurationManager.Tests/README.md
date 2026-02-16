# Testes Unitários - ConfigurationManager Singleton

## Visão Geral

Esta suíte de testes valida a implementação do padrão Singleton no ConfigurationManager, garantindo que:
- Apenas uma instância existe durante toda a execução
- A implementação é thread-safe
- As configurações são carregadas e gerenciadas corretamente
- Os serviços compartilham a mesma instância

## Estrutura dos Testes

### ConfigurationManagerTests.cs

Testes focados no comportamento do ConfigurationManager:

#### 1. **Instance_ShouldReturnSameInstance_WhenCalledMultipleTimes**
- Valida que o padrão Singleton retorna sempre a mesma instância
- Usa `Assert.Same()` para verificar que as referências são idênticas

#### 2. **Instance_ShouldBeThreadSafe_WhenAccessedFromMultipleThreads**
- Testa a thread-safety criando 10 threads simultâneas
- Verifica que todas as threads recebem a mesma instância
- Valida o uso correto de `Lazy<T>` para inicialização thread-safe

#### 3. **LoadConfigurations_ShouldLoadSettings_WhenCalledFirstTime**
- Verifica que as configurações são carregadas corretamente
- Testa valores específicos como DatabaseConnection

#### 4. **LoadConfigurations_ShouldNotLoadTwice_WhenCalledMultipleTimes**
- Valida o padrão double-check locking
- Garante que LoadConfigurations é idempotente

#### 5. **GetSetting_ShouldReturnCorrectValue_WhenKeyExists**
- Testa a recuperação de múltiplas configurações
- Valida valores de ApiKey, CacheServer e MaxRetries

#### 6. **GetSetting_ShouldReturnNull_WhenKeyDoesNotExist**
- Verifica o comportamento para chaves inexistentes
- Garante que não lança exceção

#### 7. **GetSetting_ShouldAutoLoadConfigurations_WhenNotLoadedYet**
- Testa o carregamento automático (lazy loading)
- Valida que não é necessário chamar LoadConfigurations explicitamente

#### 8. **UpdateSetting_ShouldUpdateExistingValue_WhenKeyExists**
- Verifica a atualização de configurações existentes
- Testa mudança de LogLevel de "Information" para "Debug"

#### 9. **UpdateSetting_ShouldAddNewSetting_WhenKeyDoesNotExist**
- Valida adição de novas configurações em runtime
- Testa comportamento dinâmico do dicionário

#### 10. **UpdateSetting_ShouldBeThreadSafe_WhenCalledFromMultipleThreads**
- Testa concorrência com 20 threads simultâneas
- Valida o uso correto de `lock` para sincronização
- Garante que todas as atualizações são persistidas

#### 11. **ConfigurationManager_ShouldLoadAllExpectedSettings**
- Teste de integração que valida todas as 7 configurações padrão
- Verifica a integridade completa do carregamento

### ServicesTests.cs

Testes focados na integração dos serviços com o ConfigurationManager:

#### 1. **DatabaseService_Connect_ShouldUseConfigurationManager**
- Valida que DatabaseService consegue conectar usando as configurações

#### 2. **ApiService_MakeRequest_ShouldUseConfigurationManager**
- Valida que ApiService consegue fazer requisições

#### 3. **CacheService_Connect_ShouldUseConfigurationManager**
- Valida que CacheService consegue conectar ao cache

#### 4. **LoggingService_Log_ShouldUseConfigurationManager**
- Valida que LoggingService consegue logar mensagens

#### 5. **AllServices_ShouldShareSameConfigurationManagerInstance**
- Teste de integração que valida o compartilhamento da instância
- Executa todos os serviços sequencialmente
- Garante que todos usam o mesmo Singleton

## Executando os Testes

### Via dotnet CLI
```bash
cd ConfigurationManagerSolution
dotnet test
```

### Com cobertura
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Com detalhes
```bash
dotnet test --logger "console;verbosity=detailed"
```

## Resultados Esperados

✅ **Total: 16 testes**
- ConfigurationManagerTests: 11 testes
- ServicesTests: 5 testes

Todos devem passar sem erros.

## Padrões de Teste

- **Arrange-Act-Assert (AAA)**: Todos os testes seguem este padrão
- **Nomenclatura**: `MethodName_ShouldExpectedBehavior_WhenCondition`
- **Isolamento**: Cada teste é independente
- **Clareza**: Nomes descritivos e asserts explícitos

## Tecnologias

- **Framework de Teste**: xUnit 2.9.2
- **Target Framework**: .NET 9.0
- **Cobertura**: coverlet.collector 6.0.2

## Validações de Segurança Thread-Safe

Os testes incluem validações específicas para:
- Lazy initialization com Lazy<T>
- Double-check locking pattern
- Lock statement para sincronização de escritas
- Concurrent access por múltiplas threads

## Notas Importantes

⚠️ **Singleton em Testes**: Como o ConfigurationManager é um Singleton global, alguns testes podem compartilhar estado. Isso é intencional para validar o comportamento real do padrão.

✅ **Thread-Safety**: Os testes de concorrência garantem que a implementação é segura para ambientes multi-thread.

📊 **Cobertura**: A suíte de testes cobre:
- Criação do Singleton
- Carregamento de configurações
- Leitura de configurações
- Atualização de configurações
- Thread-safety
- Integração com serviços

