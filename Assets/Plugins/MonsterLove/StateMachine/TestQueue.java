import java.util.Stack;
import java.util.NoSuchElementException;

public class TestQueue{
	public static void main(String[] args) {
		Queue<Integer> q=new Queue<Integer>();
		Integer a[]= {1,2,3,4,5};
		q.offer(a[0]);q.offer(a[1]);q.offer(a[2]);q.offer(a[3]);q.offer(a[0]);
		q.offer(a[1]);q.offer(a[1]);q.offer(a[1]);q.offer(a[1]);q.offer(a[1]);
		q.offer(0);q.offer(0);q.offer(0);q.offer(0);q.offer(0);
		q.offer(0);q.offer(0);q.offer(0);q.offer(0);q.offer(0);
		q.offer(0);q.offer(0);
			
			try{			
			    q.add(null);
			    //q.offer(null);
			    //q.add(0);			 
		    }
		    catch(IllegalStateException ex){
			    System.out.println("Queue is full");
		    }
		    catch(NullPointerException ex) {
			    System.out.println("The element can not be null");
		    }
			
			try{			
			   // q.add(null);
			    q.offer(null);
			   // q.add(0);			 
		    }
		    catch(IllegalStateException ex){
			    System.out.println("Queue is full");
		    }
		    catch(NullPointerException ex) {
			    System.out.println("the element can not be null");
		    }
			
			try{			
				   // q.add(null);
				   // q.offer(null);
				    q.add(0);			 
			    }
			    catch(IllegalStateException ex){
				    System.out.println("Queue is full");
			    }
			    catch(NullPointerException ex) {
				    System.out.println("the element can not be null");
			    }
			
			
		
			
		int[] k=new int[20];
		k[1]=q.peek();	
		q.poll(); k[2]=q.peek();
		q.poll(); k[3]=q.peek();
		q.poll(); k[4]=q.peek();
	
		System.out.println(k[1]+" "+k[2]+" "+k[3]+" "+k[4]);
		
		try{
			Queue<Integer> q1=new Queue<Integer>();
			q1.add(1);
			int c=q1.element();
			q1.poll();
			q1.remove();
			//int t=q1.element();
		}
		catch(NoSuchElementException ex){
			System.out.println("Queue is empty");
		}
		
		
		
	}
}



class Queue<E> extends Stack<E>{
    public final int dump=10;
    private  Stack<E> stk;
    public Queue( ){ 
    	super();
    	stk=new Stack<E>();	
    }
    
    public boolean add(E e) throws IllegalStateException, ClassCastException, 
    NullPointerException, IllegalArgumentException{ 
    	if(e==null) 
    		throw new NullPointerException("the element can not be null");
    	if(stk.size()<dump)
    		stk.push(e);
    	else if(super.size()==0) {
            while(stk.size()>0) {
            	E c=stk.pop();
            	super.push(c);
            }
    	}
    	else 
    		throw new IllegalStateException("queue is full");
    	return true;
    }
    
    public boolean offer(E e) throws ClassCastException, NullPointerException, 
    IllegalArgumentException{  
    	if(e==null) 
    		throw new NullPointerException("the element can not be null");
    	if(stk.size()<dump)
		    stk.push(e);
	    else if(super.size()==0) {
            while(stk.size()>0) {
        	    E c=stk.pop();
        	    super.push(c);
            }
	    }
	    else return false;
    	return true;
	} 
    
    public E remove( ) throws NoSuchElementException { 
    	if(stk.size()<=0 && super.size()<=0)
    		throw new NoSuchElementException("the queue is null");
    	else if(super.size()>0) {
    		E c=super.pop();
    		return c;
    	}
    	else{
    		while(stk.size()>0) {
         	   E c=stk.pop();
         	   super.push(c);
            }
    		E c=super.pop();
    		return c;
    	}
    	
    }
    
    public E poll( ) { 
    	if(stk.size()<=0 && super.size()<=0)
    		return null;
    	else if(super.size()>0) {
    		E c=super.pop();
    		return c;
    	}
    	else{
    		while(stk.size()>0) {
         	   E c=stk.pop();
         	   super.push(c);
            }
    		E c=super.pop();
    		return c;
    	}
    }
    
    public E peek ( ) {  
    	if(stk.size()<=0 && super.size()<=0)
    		return null;
    	else if(super.size()>0) {
    		E c=super.peek();
    		return c;
    	}
    	else{
    		while(stk.size()>0) {
         	   E c=stk.pop();
         	   super.push(c);
            }
    		E c=super.peek();
    		return c;
    	}
    }
    
    public E element( ) throws NoSuchElementException { 
    	if(stk.size()<=0 && super.size()<=0)
    		throw new NoSuchElementException("the queue is null");
    	else if(super.size()>0) {
    		E c=super.peek();
    		return c;
    	}
    	else{
    		while(stk.size()>0) {
         	   E c=stk.pop();
         	   super.push(c);
            }
    		E c=super.peek();
    		return c;
    	}
    	
    }
}
