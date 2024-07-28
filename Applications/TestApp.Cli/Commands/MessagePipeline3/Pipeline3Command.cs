namespace TestApp.Cli.Commands.MessagePipeline3;

/** _note: 
 * This work might have implemented part of a monad.
 * 
 * Quote:
 *      From: https://samgrayson.me/essays/monads-as-a-programming-pattern/
 *      
 *      A monad is a type that wraps an object of another type. There is no
 *      direct way to get that ‘inside’ object. Instead you ask the monad to
 *      act on it for you. Monadic classes are a lot like classes implementing
 *      the visitor pattern, but monads are capable of returning something
 *      wrapped in another monad.
 */

public class Pipeline3Command
{
    public void Do1()
    {
        new Flow1()
            .DoAction1_1()?
            .DoAction1_2()?
            .DoAction1_3()?

            .DoAction2_1()?
            .DoAction2_2()?
            .DoAction2_3()?

            .DoAction3_1()?
            .DoAction3_2()?
            .DoAction3_3();
    }

    public void Do2()
    {
        var flow1 = new Flow1()
            .DoAction1_1()
            .DoAction1_2();

        if(flow1.Msg1.Value > 0)
        {
            new Flow2()
                .DoAction2_1()?
                .DoAction2_2();
        }
        else
        {
            new Flow3()
                .DoAction3_1()?
                .DoAction3_2()?
                .DoAction3_3();
        }
    }
}
