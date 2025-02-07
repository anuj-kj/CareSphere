import { useForm } from 'react-hook-form';

const Contact = () => {
    const { register, handleSubmit, formState: { errors } } = useForm();

    const onSubmit = (data: any) => {
        console.log(data);
    };

    return (
        <div className="container py-5">
            <h1>Contact Page</h1>
            <form onSubmit={handleSubmit(onSubmit)}>
                <div className="mb-3">
                    <label htmlFor="name" className="form-label">Name</label>
                    <input
                        type="text"
                        className={`form-control ${errors.name ? 'is-invalid' : ''}`}
                        id="name"
                        {...register('name', { required: 'Name is required' })}
                    />
                    {errors.name && <div className="invalid-feedback">{String(errors.name.message)}</div>}
                </div>
                <div className="mb-3">
                    <label htmlFor="email" className="form-label">Email</label>
                    <input
                        type="email"
                        className={`form-control ${errors.email ? 'is-invalid' : ''}`}
                        id="email"
                        {...register('email', { required: 'Email is required' })}
                    />
                    {errors.email && <div className="invalid-feedback">{String(errors.email.message)}</div>}
                </div>
                <div className="mb-3">
                    <label htmlFor="phone" className="form-label">Phone</label>
                    <input
                        type="tel"
                        className={`form-control ${errors.phone ? 'is-invalid' : ''}`}
                        id="phone"
                        {...register('phone', { required: 'Phone number is required' })}
                    />
                    {errors.phone && <div className="invalid-feedback">{String(errors.phone.message)}</div>}
                </div>
                <button type="submit" className="btn btn-primary">Submit</button>
            </form>
        </div>
    );
};

export default Contact;