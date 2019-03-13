# LogAppender
Prototyp s ukázkou použití paramtrů pro logování v knihovně **log4net**.

## Ukázková funkčnost
* konfigurace oddělená do samostatného souboru
* použítí vlastního appenderu a jeho konfigurace
* rozdělení logování do samostatných souborů podle úrovně logování - loglevel
* použití komplexnějších "pattern layoutů"
* využití životního cyklu web aplikace pro naplnění parametrů pro logování: GlobalContext a ThreadContext
* použití NDC (nested diagnostic context) pro rozlišení zápisů v logu
