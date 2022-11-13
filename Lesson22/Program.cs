using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lesson22
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите размерность массива");
            int n = Convert.ToInt32(Console.ReadLine());

            Func<object, int[]> func1 = new Func<object, int[]>(GetArray);        
            Task<int[]> task1 = new Task<int[]>(func1, n);                               // Задача формирования массива

            Func<Task<int[]>, int> func2 = new Func<Task<int[]>, int>(SumArray);
            Task<int> task2 = task1.ContinueWith<int>(func2);                           // Задача суммирования массива

            Action<Task<int[]>> action = new Action<Task<int[]>>(PrintArray);  
            Task task3 = task1.ContinueWith(action);                                    //Задача вывода массива

            Action<Task<int>> action2 = new Action<Task<int>>(PrintSum);
            Task task4 = task2.ContinueWith(action2);                                   //Задача вывода суммы массива и максимального числа в массиве

            task1.Start();
            Console.ReadKey();
        }

        static int[] GetArray(object a)                       // Метод формирования массива
        {
            int n = (int)a;
            int[] array = new int[n];
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                array[i] = random.Next(0, 100);
            }
            return array;
        }                                                       
        static int SumArray(Task<int[]> task)                     //Метод суммы массива
        {
            int sum = 0;
            int[] array = task.Result;
            for (int i = 0; i < array.Count(); i++)
            {
                sum += array[i];
            }
            return sum;
        }
        static void PrintArray(Task<int[]> task)                  // Метод вывода массива и максимального числа в массиве
        {
            int max = task.Result.Max();
            int[] array = task.Result;
            for (int i = 0; i < array.Count(); i++)
            {
                Console.Write($"\n{array[i]} ");
            }
            Console.Write($"\nМаксимальное число: {max} ");
        }
        static void PrintSum(Task<int> task)                      // Метод вывода суммы массива
        {
            int sum = task.Result;
            Console.Write($"\nСумма чисел массива: {sum} ");
        }                   
    }
}
