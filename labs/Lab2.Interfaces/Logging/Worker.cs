namespace Lab2.Interfaces.Logging;

public static class Worker
{
    public static void DoWork(ILogger logger)
    {
        logger.Log("Работа начата");
        for (var step = 1; step <= 3; step++)
        {
            logger.Log($"Выполнен шаг {step} из 3");
        }

        logger.Log("Работа завершена");
    }
}
